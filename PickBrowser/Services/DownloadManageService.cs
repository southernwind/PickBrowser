using System.Collections.Generic;
using System.Reactive.Concurrency;

using PickBrowser.Models.Network.Objects;
using PickBrowser.Services.Objects;

using ScrapingLibrary;

namespace PickBrowser.Services; 
public class DownloadManageService {
	private readonly HttpClientWrapper _hc;
	public ReactiveCollection<DownloadTask> DownloadQueue {
		get;
	} = new();

	public ReactiveCollection<DownloadTask> RunningDownloadQueue {
		get;
	} = new();

	public int MaxRunningTaskCount { get; } = 10;

	public DownloadManageService(HttpClientWrapper hc) {
		this._hc = hc;
		this.DownloadQueue
			.ObserveAddChanged()
			.ToUnit()
			.Merge(
				this.RunningDownloadQueue
					.ObserveRemoveChanged()
					.ToUnit()
			)
			.Merge(
				this.RunningDownloadQueue
					.ObserveElementObservableProperty(x => x.HasError)
					.ToUnit()
			)
			.Throttle(new TimeSpan(0, 0, 0, 0, 500))
			.Subscribe(x => {
				lock (this) {
					var count = this.MaxRunningTaskCount - this.RunningDownloadQueue.Where(x => !x.HasError.Value).Count();
					if (count > 0) {
						var tasks = this.DownloadQueue.Where(x => !x.IsCompleted.Value).Except(this.RunningDownloadQueue).OrderBy(x => x.Priority.Value).Take(count);
						if (tasks == null || tasks.IsEmpty()) {
							return;
						}
						this.RunningDownloadQueue.AddRange(tasks);
					}
				}
			});

		this.RunningDownloadQueue
			.ObserveAddChanged()
			.ObserveOn(TaskPoolScheduler.Default)
			.Subscribe(async x => {
				await x.ExecuteAsync();
				x.IsCompleted.Subscribe(isCompleted => {
					if (!isCompleted) {
						return;
					}
					lock (this) {
						this.RunningDownloadQueue.Remove(x);
					}
				});
			});
	}

	public void AddDownloadQueue(IEnumerable<NetworkRequest> networkRequests) {
		lock (this) {
			this.DownloadQueue.AddRange(networkRequests.Select(x => new DownloadTask(x, this._hc)));
		}
	}
}
