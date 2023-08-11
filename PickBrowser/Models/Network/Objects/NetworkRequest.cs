using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Fiddler;

namespace PickBrowser.Models.Network.Objects;

public partial class NetworkRequest : ObservableObject {
	[ObservableProperty]
	private HTTPResponseHeaders _responseHeaders;
	public int SessionId {
		get;
	}

	public string DocumentTitle {
		get;
	}

	public string RequestUrl {
		get;
	}

	public string AbsoluteUrl {
		get;
	}

	public string Method {
		get;
	}

	public Dictionary<string, string> GeneralHeaders {
		get;
	} = new();

	public HTTPRequestHeaders RequestHeaders {
		get;
	}

	public NetworkRequest(string documentTitle,Session session) {
		this.SessionId = session.id;
		this.DocumentTitle = documentTitle;
		this.RequestUrl = session.url;
		this.AbsoluteUrl = session.fullUrl;
		this.Method = session.RequestMethod;
		this.GeneralHeaders.Add("Request Url",session.fullUrl);
		this.GeneralHeaders.Add("Request Method", session.RequestMethod);
		this.GeneralHeaders.Add("Status",session.state.ToString());
		this.RequestHeaders = session.RequestHeaders;
		this.ResponseHeaders = session.ResponseHeaders;
	}

	public void Update(Session session) {
		this.ResponseHeaders = session.ResponseHeaders;
	}
}
