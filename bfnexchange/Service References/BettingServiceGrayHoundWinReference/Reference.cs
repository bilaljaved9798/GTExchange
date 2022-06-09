﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.BettingServiceGrayHoundWinReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceGrayHoundWinReference.IBettingServiceGrayHoundWin")]
    public interface IBettingServiceGrayHoundWin {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceGrayHoundWin/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceGrayHoundWin/GetDataFromBetfairReadOnlyResponse" +
            "")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IBettingServiceGrayHoundWin/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceGrayHoundWin/GetDataFromBetfairReadOnlyResponse" +
            "")]
        System.IAsyncResult BeginGetDataFromBetfairReadOnly(System.AsyncCallback callback, object asyncState);
        
        void EndGetDataFromBetfairReadOnly(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceGrayHoundWin/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceGrayHoundWin/GetCurrentMarketBookCricketRespons" +
            "e")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IBettingServiceGrayHoundWin/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceGrayHoundWin/GetCurrentMarketBookCricketRespons" +
            "e")]
        System.IAsyncResult BeginGetCurrentMarketBookCricket(string Password, System.AsyncCallback callback, object asyncState);
        
        void EndGetCurrentMarketBookCricket(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServiceGrayHoundWinChannel : bfnexchange.BettingServiceGrayHoundWinReference.IBettingServiceGrayHoundWin, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServiceGrayHoundWinClient : System.ServiceModel.ClientBase<bfnexchange.BettingServiceGrayHoundWinReference.IBettingServiceGrayHoundWin>, bfnexchange.BettingServiceGrayHoundWinReference.IBettingServiceGrayHoundWin {
        
        private BeginOperationDelegate onBeginGetDataFromBetfairReadOnlyDelegate;
        
        private EndOperationDelegate onEndGetDataFromBetfairReadOnlyDelegate;
        
        private System.Threading.SendOrPostCallback onGetDataFromBetfairReadOnlyCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetCurrentMarketBookCricketDelegate;
        
        private EndOperationDelegate onEndGetCurrentMarketBookCricketDelegate;
        
        private System.Threading.SendOrPostCallback onGetCurrentMarketBookCricketCompletedDelegate;
        
        public BettingServiceGrayHoundWinClient() {
        }
        
        public BettingServiceGrayHoundWinClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServiceGrayHoundWinClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceGrayHoundWinClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceGrayHoundWinClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> GetDataFromBetfairReadOnlyCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> GetCurrentMarketBookCricketCompleted;
        
        public void GetDataFromBetfairReadOnly() {
            base.Channel.GetDataFromBetfairReadOnly();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetDataFromBetfairReadOnly(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetDataFromBetfairReadOnly(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndGetDataFromBetfairReadOnly(System.IAsyncResult result) {
            base.Channel.EndGetDataFromBetfairReadOnly(result);
        }
        
        private System.IAsyncResult OnBeginGetDataFromBetfairReadOnly(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginGetDataFromBetfairReadOnly(callback, asyncState);
        }
        
        private object[] OnEndGetDataFromBetfairReadOnly(System.IAsyncResult result) {
            this.EndGetDataFromBetfairReadOnly(result);
            return null;
        }
        
        private void OnGetDataFromBetfairReadOnlyCompleted(object state) {
            if ((this.GetDataFromBetfairReadOnlyCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetDataFromBetfairReadOnlyCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetDataFromBetfairReadOnlyAsync() {
            this.GetDataFromBetfairReadOnlyAsync(null);
        }
        
        public void GetDataFromBetfairReadOnlyAsync(object userState) {
            if ((this.onBeginGetDataFromBetfairReadOnlyDelegate == null)) {
                this.onBeginGetDataFromBetfairReadOnlyDelegate = new BeginOperationDelegate(this.OnBeginGetDataFromBetfairReadOnly);
            }
            if ((this.onEndGetDataFromBetfairReadOnlyDelegate == null)) {
                this.onEndGetDataFromBetfairReadOnlyDelegate = new EndOperationDelegate(this.OnEndGetDataFromBetfairReadOnly);
            }
            if ((this.onGetDataFromBetfairReadOnlyCompletedDelegate == null)) {
                this.onGetDataFromBetfairReadOnlyCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetDataFromBetfairReadOnlyCompleted);
            }
            base.InvokeAsync(this.onBeginGetDataFromBetfairReadOnlyDelegate, null, this.onEndGetDataFromBetfairReadOnlyDelegate, this.onGetDataFromBetfairReadOnlyCompletedDelegate, userState);
        }
        
        public void GetCurrentMarketBookCricket(string Password) {
            base.Channel.GetCurrentMarketBookCricket(Password);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetCurrentMarketBookCricket(string Password, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetCurrentMarketBookCricket(Password, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndGetCurrentMarketBookCricket(System.IAsyncResult result) {
            base.Channel.EndGetCurrentMarketBookCricket(result);
        }
        
        private System.IAsyncResult OnBeginGetCurrentMarketBookCricket(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string Password = ((string)(inValues[0]));
            return this.BeginGetCurrentMarketBookCricket(Password, callback, asyncState);
        }
        
        private object[] OnEndGetCurrentMarketBookCricket(System.IAsyncResult result) {
            this.EndGetCurrentMarketBookCricket(result);
            return null;
        }
        
        private void OnGetCurrentMarketBookCricketCompleted(object state) {
            if ((this.GetCurrentMarketBookCricketCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetCurrentMarketBookCricketCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetCurrentMarketBookCricketAsync(string Password) {
            this.GetCurrentMarketBookCricketAsync(Password, null);
        }
        
        public void GetCurrentMarketBookCricketAsync(string Password, object userState) {
            if ((this.onBeginGetCurrentMarketBookCricketDelegate == null)) {
                this.onBeginGetCurrentMarketBookCricketDelegate = new BeginOperationDelegate(this.OnBeginGetCurrentMarketBookCricket);
            }
            if ((this.onEndGetCurrentMarketBookCricketDelegate == null)) {
                this.onEndGetCurrentMarketBookCricketDelegate = new EndOperationDelegate(this.OnEndGetCurrentMarketBookCricket);
            }
            if ((this.onGetCurrentMarketBookCricketCompletedDelegate == null)) {
                this.onGetCurrentMarketBookCricketCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetCurrentMarketBookCricketCompleted);
            }
            base.InvokeAsync(this.onBeginGetCurrentMarketBookCricketDelegate, new object[] {
                        Password}, this.onEndGetCurrentMarketBookCricketDelegate, this.onGetCurrentMarketBookCricketCompletedDelegate, userState);
        }
    }
}
