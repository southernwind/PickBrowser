using CommunityToolkit.Mvvm.ComponentModel;

using Fiddler;

using PickBrowser.Models.Network.Objects;

using System.Collections.Generic;

namespace PickBrowser.ViewModels;
public class NetworkRequestViewModel : ObservableObject {

	public NetworkRequest NetworkRequest {
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
	public HTTPResponseHeaders ResponseHeaders {
		get {
			return this.NetworkRequest.ResponseHeaders;
		}
	}

	public ReactiveProperty<bool> Checked {
		get;
	} = new();

	public NetworkRequestViewModel(NetworkRequest networkRequest) {
		this.NetworkRequest = networkRequest;
		this.RequestUrl = networkRequest.RequestUrl;
		this.AbsoluteUrl = networkRequest.AbsoluteUrl;
		this.Method = networkRequest.Method;
		this.GeneralHeaders = networkRequest.GeneralHeaders;
		this.RequestHeaders = networkRequest.RequestHeaders;
		networkRequest.PropertyChanged += (_, x) => {
			this.OnPropertyChanged(x.PropertyName);
		};
	}
}
