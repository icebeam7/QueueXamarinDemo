using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QueueXamarinDemo.Services;

namespace QueueXamarinDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QueuePage : ContentPage
	{
		public QueuePage ()
		{
			InitializeComponent ();
		}

        public async void btnAddMessage_Clicked(object sender, EventArgs e)
        {
            var message = txtData.Text;
            await QueueService.EnqueueMessage(message);
            lblData.Text = "Message inserted in the queue";
            txtData.Text = string.Empty;
        }

        public async void btnGetNextMessage_Clicked(object sender, EventArgs e)
        {
            lblData.Text = await QueueService.PeekAtNextMessage();
        }

        public async void btnChangeNextMessage_Clicked(object sender, EventArgs e)
        {
            var message = txtData.Text;
            await QueueService.ChangeContentNextMessage(message);
            lblData.Text = "Message changed";
            txtData.Text = string.Empty;
        }

        public async void btnGetMessageDelete_Clicked(object sender, EventArgs e)
        {
            lblData.Text = await QueueService.DequeueMessage();
        }

        public async void btnGetQueueLength_Clicked(object sender, EventArgs e)
        {
            lblData.Text = $"{await QueueService.GetQueueLength()} items in the queue";
        }
    }
}