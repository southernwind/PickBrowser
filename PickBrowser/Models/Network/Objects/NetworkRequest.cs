using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Fiddler;

namespace PickBrowser.Models.Network.Objects;

public partial class NetworkRequest : ObservableObject {
	[ObservableProperty]
	private string _requestUrl;

	[ObservableProperty]
	private string _absoluteUrl;

	[ObservableProperty]
	private string _method;

	public Dictionary<string, string> GeneralHeaders {
		get;
	} = new();

	public HTTPRequestHeaders RequestHeaders {
		get;
	}
	public HTTPResponseHeaders ResponseHeaders {
		get;
	}

	public NetworkRequest(Session session) {
		this.RequestUrl = session.url;
		this._absoluteUrl = session.fullUrl;
		this._method = session.RequestMethod;
		this.GeneralHeaders.Add("Request Url",session.fullUrl);
		this.GeneralHeaders.Add("Request Method", session.RequestMethod);
		this.GeneralHeaders.Add("Status",session.state.ToString());
		this.RequestHeaders = session.RequestHeaders;
		this.ResponseHeaders = session.ResponseHeaders;
	}
}
