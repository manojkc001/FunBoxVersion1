using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_Application.Services;
using Bot_Application.Serialization;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
        //    private static async void Response(IDialogContext context, Activity message)
        //{
          //  Activity activity = new Activity();
            var response = await Luis.GetResponse(activity.Text);

            if (response != null)
            {
                var intent = new Intent();
                var entity = new Entity1();

                string laptop = string.Empty;
                string atm = string.Empty;
                string service = string.Empty;

                foreach (var item in response.entities)
                {
                    switch (item.type.ToLower())
                    {
                        case "laptop repair":
                            laptop = item.entity;
                            break;
                        case "atm machine":
                            atm = item.entity;
                            break;
                        case "service line":
                            service = item.entity;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(laptop))
                {
                    // return our reply to the user
                    await context.PostAsync($"Hello, {laptop} available to A-3F, B-2F, D-3F");
                }
                else if (!string.IsNullOrEmpty(atm))
                {
                    await context.PostAsync($"Hello, {atm} available to A-Ground Floor, D-Cafeteria");
                }
                else if (!string.IsNullOrEmpty(service))
                {
                    await context.PostAsync($"Hello, {service} are SI, DD, AI ");
                }
                else
                    await context.PostAsync($"No result found");
            }
            
        }
    }
}