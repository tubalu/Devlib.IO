﻿//-----------------------------------------------------------------------
// <copyright file="WcfMessageInspectorEndpointBehavior.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ServiceModel
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Threading;

    /// <summary>
    /// WcfMessageInspector EndpointBehavior.
    /// </summary>
    [Serializable]
    public class WcfMessageInspectorEndpointBehavior : Attribute, IEndpointBehavior
    {
        /// <summary>
        /// Field _serviceHostBase.
        /// </summary>
        [NonSerialized]
        private readonly ServiceHostBase _serviceHostBase;

        /// <summary>
        /// Field _clientCredentials.
        /// </summary>
        [NonSerialized]
        private readonly ClientCredentials _clientCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfMessageInspectorEndpointBehavior"/> class.
        /// </summary>
        public WcfMessageInspectorEndpointBehavior()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfMessageInspectorEndpointBehavior" /> class.
        /// </summary>
        /// <param name="clientCredentials">The client credentials.</param>
        public WcfMessageInspectorEndpointBehavior(ClientCredentials clientCredentials)
        {
            this._clientCredentials = clientCredentials;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfMessageInspectorEndpointBehavior" /> class.
        /// </summary>
        /// <param name="serviceHostBase">The service host.</param>
        public WcfMessageInspectorEndpointBehavior(ServiceHostBase serviceHostBase)
        {
            this._serviceHostBase = serviceHostBase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfMessageInspectorEndpointBehavior" /> class.
        /// </summary>
        /// <param name="clientCredentials">The client credentials.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        public WcfMessageInspectorEndpointBehavior(ClientCredentials clientCredentials, ServiceHostBase serviceHostBase)
        {
            this._clientCredentials = clientCredentials;
            this._serviceHostBase = serviceHostBase;
        }

        /// <summary>
        /// Occurs before send request.
        /// </summary>
        public event EventHandler<WcfMessageInspectorEventArgs> SendingRequest;

        /// <summary>
        /// Occurs after receive reply.
        /// </summary>
        public event EventHandler<WcfMessageInspectorEventArgs> ReceivingReply;

        /// <summary>
        /// Occurs after receive request.
        /// </summary>
        public event EventHandler<WcfMessageInspectorEventArgs> ReceivingRequest;

        /// <summary>
        /// Occurs before send reply.
        /// </summary>
        public event EventHandler<WcfMessageInspectorEventArgs> SendingReply;

        /// <summary>
        /// Occurs when has error.
        /// </summary>
        public event EventHandler<WcfErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Gets or sets a value indicating whether ignore message inspection.
        /// </summary>
        public bool IgnoreMessageInspect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether ignore message validation.
        /// </summary>
        public bool IgnoreMessageValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            WcfMessageInspector inspector = new WcfMessageInspector(endpoint, this._clientCredentials);

            inspector.IgnoreMessageInspect = this.IgnoreMessageInspect;
            inspector.IgnoreMessageValidate = this.IgnoreMessageValidate;

            inspector.SendingRequest += (s, e) => this.RaiseEvent(this.SendingRequest, endpoint, e);
            inspector.ReceivingReply += (s, e) => this.RaiseEvent(this.ReceivingReply, endpoint, e);
            inspector.ErrorOccurred += (s, e) => this.RaiseEvent(this.ErrorOccurred, e);

            clientRuntime.MessageInspectors.Add(inspector);
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            WcfMessageInspector inspector = new WcfMessageInspector(endpoint, this._serviceHostBase);

            inspector.IgnoreMessageInspect = this.IgnoreMessageInspect;
            inspector.IgnoreMessageValidate = this.IgnoreMessageValidate;

            inspector.ReceivingRequest += (s, e) => this.RaiseEvent(this.ReceivingRequest, endpoint, e);
            inspector.SendingReply += (s, e) => this.RaiseEvent(this.SendingReply, endpoint, e);
            inspector.ErrorOccurred += (s, e) => this.RaiseEvent(this.ErrorOccurred, e);

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);

            WcfErrorHandler errorHandler = new WcfErrorHandler();

            errorHandler.ErrorOccurred += (s, e) => this.RaiseEvent(this.ErrorOccurred, e);

            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(errorHandler);
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Method RaiseEvent.
        /// </summary>
        /// <param name="eventHandler">Instance of EventHandler.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="e">The <see cref="WcfMessageInspectorEventArgs" /> instance containing the event data.</param>
        private void RaiseEvent(EventHandler<WcfMessageInspectorEventArgs> eventHandler, ServiceEndpoint endpoint, WcfMessageInspectorEventArgs e)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety.
            EventHandler<WcfMessageInspectorEventArgs> temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(this, new WcfMessageInspectorEventArgs(e.Message, e.MessageId, e.IsOneWay, e.ValidationError, endpoint, this._clientCredentials, this._serviceHostBase));
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="e">The <see cref="WcfErrorEventArgs"/> instance containing the event data.</param>
        private void RaiseEvent(EventHandler<WcfErrorEventArgs> eventHandler, WcfErrorEventArgs e)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety.
            EventHandler<WcfErrorEventArgs> temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(this, e);
            }
        }
    }
}
