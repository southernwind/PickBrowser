using System.Reactive.Linq;

using Fiddler;

namespace PickBrowser.Services;

internal class ProxyService {
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

	public ProxyService() {
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
	}

	public void Start() {
		// 証明書インストール
		if (!CertMaker.rootCertExists()) {
			CertMaker.createRootCert();
			CertMaker.trustRootCert();
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
