using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XamarinAdvanced.Triggers;

namespace XamarinAdvanced.Controls
{
    #region MultiTrigger Commented
    public class IsSelectTrigger : TriggerAction<SwitchGroupLayout>
    { 
        protected override void Invoke(SwitchGroupLayout sender)
        {
            MultiTrigger _trigger = new MultiTrigger(typeof(SwitchGroupLayout));
            BindingCondition _condition = new BindingCondition() {
                
            };
            List<Switch> lista = new List<Switch>();
            var _this = sender as Layout<View>;

            foreach (var view in _this.Children)
            {
                if (view is Switch && view != sender)
                {
                    /*
                    _trigger.Conditions.Add(new BindingCondition() {

                        Binding:((Switch)view).BindingContext.ToString(),source:"IsToggled"

                    });
                    //lista.Add((Switch)view);*/
                }
            }
            //_trigger.Conditions.Add();
        }
    }
    #endregion

    public class ToggledEventTriggerSG : TriggerAction<Switch>
    {
        protected override void Invoke(Switch sender)
        {
            List<Switch> lista = new List<Switch>();
            var parent = sender.Parent as Layout<View>;
            var SwitchGroupParent = sender.Parent as SwitchGroupLayout;

            //Caso o trigger seja chamado ao IsToggled==false verifica se há algum switch.IsToggled==true
            //se não houver IsSelect=false e não executa o resto da função
            if (sender.IsToggled == false)
            {
                var _isSelect = false;
                //Se não há nenhum switch selecionado então propriedade IsSelect = false
                foreach (var view in parent.Children)
                {
                    if (view is Switch)
                    {
                        if(((Switch)view).IsToggled == true)
                        {
                            _isSelect = true;
                        }
                    }
                }
                SwitchGroupParent.IsSelect = _isSelect;
                return;
            }
            else
            {
                //Se foi chamado pelo IsToggled==true então IsSelect=true e continua a execução do trigger
                SwitchGroupParent.IsSelect = true;
            }
            

            foreach (var view in parent.Children)
            {
                
                if (view is Switch && view != sender)
                {
                    lista.Add((Switch)view);
                    ((Switch)view).IsToggled = false;
                }else if(view is Switch)
                {
                    lista.Add((Switch)view);
                }
            }

            //Se opção correta foi atríbuida verifica se o switch selecionado é o de index igual ao da propriedade opcaoCorreta
            if (!SwitchGroupParent.OpcaoCorretaIndex.Equals("null"))
            {
               
                var correta = Convert.ToInt32(SwitchGroupParent.OpcaoCorretaIndex);
                string error = "";
                try
                {
                    if (sender.Equals(lista[correta]))
                    {
                        SwitchGroupParent.isCorrect = true;
                    }
                    else
                    {
                        SwitchGroupParent.isCorrect = false;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    error += ex.Message;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    error += ex.Message;
                }

                if (!String.IsNullOrEmpty(error))
                    App.Current.MainPage.DisplayAlert("Error",
                                                      "O valor da opção correta não é um index válido: \n" + error,
                                                      "Ok");

            }

        }
    }
    public class SwitchGroupLayout: StackLayout
    {
        #region Properties
        
        public static readonly BindableProperty OpcaoCorretaIndexProperty =
            BindableProperty.Create(nameof(OpcaoCorretaIndex), 
                                    typeof(string), 
                                    typeof(SwitchGroupLayout),
                                    defaultValue:"null");
        public string OpcaoCorretaIndex
        {
            get => (string)GetValue(OpcaoCorretaIndexProperty);
            set { SetValue(OpcaoCorretaIndexProperty, value); OnPropertyChanged(); }
        }
        
        public static readonly BindableProperty IsCorrectProperty =
            BindableProperty.Create(nameof(isCorrect), 
                                    typeof(bool), 
                                    typeof(SwitchGroupLayout),
                                    defaultValue:false);

        public bool isCorrect
        {
            get => (bool)GetValue(IsCorrectProperty);
            set { SetValue(IsCorrectProperty, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty IsSelectProperty =
             BindableProperty.Create(nameof(isCorrect), 
                                    typeof(bool), 
                                    typeof(SwitchGroupLayout), 
                                    defaultValue:false);

        public bool IsSelect
        {
            get => (bool)GetValue(IsSelectProperty);
            set { SetValue(IsSelectProperty, value); OnPropertyChanged();  }
        }

        #endregion


        public SwitchGroupLayout() {
            
            var _trigger = new EventTrigger();
            ToggledEventTriggerSG trigerzinho = new ToggledEventTriggerSG();
            _trigger.Actions.Add(trigerzinho);
            _trigger.Event = "Toggled";

            var OneSwitchStyle = new Style(typeof(Switch));
           
            OneSwitchStyle.Triggers.Add(_trigger);

            this.Resources.Add(OneSwitchStyle);

        }
        

        


    }
}
