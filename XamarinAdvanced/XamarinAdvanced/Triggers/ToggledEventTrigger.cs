using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinAdvanced.Triggers
{
    public class ToggledEventTrigger : TriggerAction<Switch>
    {
        protected override void Invoke(Switch sender)
        {
            if (sender.IsToggled == false) return;
            var parent = sender.Parent as Layout<View>;

            foreach(var view in parent.Children)
            {
                if(view is Switch && view != sender )
                { 
                    ((Switch)view).IsToggled = false;
                }
            }

        }
    }
}
