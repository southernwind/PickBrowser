using System.IO;
using System.Text.RegularExpressions;

using Fiddler;

namespace PickBrowser.Services;

public class ProxyService {
	private readonly string[] _blockDomainList;
	private readonly ConfigManageService _configManageService;
	public ReactiveProperty<Session?> AfterSessionComplete {
		get;
	}
	public ReactiveProperty<Session?> BeforeRequest {
		get;
	}
	public ReactiveProperty<ConnectionEventArgs?> AfterSocketAccept {
		get;
	}
	public ReactiveProperty<ConnectionEventArgs?> AfterSocketConnect {
		get;
	}
	public ReactiveProperty<Session?> BeforeResponse {
		get;
	}
	public ReactiveProperty<Session?> BeforeReturningError {
		get;
	}
	public ReactiveProperty<WebSocketMessageEventArgs?> OnWebSocketMessage {
		get;
	}
	public ReactiveProperty<Session?> RequestHeadersAvailable {
		get;
	}
	public ReactiveProperty<Session?> ResponseHeadersAvailable {
		get;
	}

	public ProxyService(ConfigManageService configManageService) {
		this._configManageService = configManageService;


		// ドメインブロック
		var domains = File.ReadAllLines("Assets\\BlockDomains.txt").ToList();
		if (Directory.Exists("Assets\\BlockDomains")) {
			foreach (var file in Directory.EnumerateFiles("Assets\\BlockDomains").Where(x => x.EndsWith(".txt"))) {
				domains.AddRange(File.ReadAllLines(file));
			}
		}
		this._blockDomainList = domains.OrderBy(x => x).ToArray();

		this.AfterSessionComplete = Observable.FromEvent<SessionStateHandler, Session>(
			h => (e) => h(e),
			h => FiddlerApplication.AfterSessionComplete += h,
			h => FiddlerApplication.AfterSessionComplete -= h)
			.ToReactiveProperty();

		this.AfterSocketAccept = Observable.FromEvent<EventHandler<ConnectionEventArgs>, ConnectionEventArgs>(
			h => (s,e) => h(e),
			h => FiddlerApplication.AfterSocketAccept += h,
			h => FiddlerApplication.AfterSocketAccept -= h)
			.ToReactiveProperty();


		this.AfterSocketConnect = Observable.FromEvent<EventHandler<ConnectionEventArgs>, ConnectionEventArgs>(
			h => (s, e) => h(e),
			h => FiddlerApplication.AfterSocketConnect += h,
			h => FiddlerApplication.AfterSocketConnect -= h)
			.ToReactiveProperty();

		this.BeforeResponse = Observable.FromEvent<SessionStateHandler, Session>(
		   h => (e) => h(e),
		   h => FiddlerApplication.BeforeResponse += h,
		   h => FiddlerApplication.BeforeResponse -= h)
		   .ToReactiveProperty();

		this.BeforeReturningError = Observable.FromEvent<SessionStateHandler, Session>(
		   h => (e) => h(e),
		   h => FiddlerApplication.BeforeReturningError += h,
		   h => FiddlerApplication.BeforeReturningError -= h)
		   .ToReactiveProperty();

		this.BeforeRequest = Observable.FromEvent<SessionStateHandler, Session>(
		   h => (e) => h(e),
		   h => FiddlerApplication.BeforeRequest += h,
		   h => FiddlerApplication.BeforeRequest -= h)
		   .ToReactiveProperty();

		this.OnWebSocketMessage = Observable.FromEvent<EventHandler<WebSocketMessageEventArgs>, WebSocketMessageEventArgs>(
		   h => (s, e) => h(e),
		   h => FiddlerApplication.OnWebSocketMessage += h,
		   h => FiddlerApplication.OnWebSocketMessage -= h)
		   .ToReactiveProperty();

		this.RequestHeadersAvailable = Observable.FromEvent<SessionStateHandler, Session>(
		   h => (e) => h(e),
		   h => FiddlerApplication.RequestHeadersAvailable += h,
		   h => FiddlerApplication.RequestHeadersAvailable -= h)
		   .ToReactiveProperty();

		this.ResponseHeadersAvailable = Observable.FromEvent<SessionStateHandler, Session>(
		   h => (e) => h(e),
		   h => FiddlerApplication.ResponseHeadersAvailable += h,
		   h => FiddlerApplication.ResponseHeadersAvailable -= h)
		   .ToReactiveProperty();

		this.BeforeRequest.Where(x => x != null).Subscribe(x => {
			var domain = Regex.Replace(x!.url, "/.*$", "");
			if (this._blockDomainList.Any(x =>domain.EndsWith(x))) {
				x.Abort();
			}
		});
	}

	public void Start() {
		FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.key", this._configManageService.Config.CertConfig.Key.Value);
		FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.cert", this._configManageService.Config.CertConfig.Cert.Value);

		// 証明書インストール
		if (!CertMaker.rootCertExists()) {
			if (!CertMaker.createRootCert()) {
				throw new Exception("create root cert failed");
			}
			if (!CertMaker.trustRootCert()) {
				throw new Exception("trust root cert failed");
			}

			this._configManageService.Config.CertConfig.Key.Value = FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.key", null);
			this._configManageService.Config.CertConfig.Cert.Value = FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.cert", null);
			this._configManageService.Save();
		}

		// プロキシサーバー開始
		FiddlerApplication.Startup(
			new FiddlerCoreStartupSettingsBuilder()
				.ListenOnPort(23081)
				.ChainToUpstreamGateway()
				.DecryptSSL()
				.OptimizeThreadPool()
				.Build());
	}

}
