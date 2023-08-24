//=============================================================================
// Siemens AG
// (c)Copyright (2017) All Rights Reserved
//----------------------------------------------------------------------------- 
// Tested with: Windows 10 Enterprise x64
// Engineering: Visual Studio 2013
// Functionality: Wrapps up important classes/methods of the OPC UA .NET Stack (Core/Client) to help
// with simple client implementations
//-----------------------------------------------------------------------------
// Change log table:
// Version Date Expert in charge Changes applied
// 01.00.00 31.08.2016 (Siemens) First released version
// 01.01.00 22.02.2017 (Siemens) Implements user authentication, SHA256 Cert, Basic256Rsa256 connection, read/write structs/UDTs
// 01.02.00 14.12.2017 (Siemens) Implements method calling
// 01.03.00 01.06.2018 (Siemens) Implements region Namespace; Improved parsing for complex data types; Minor bug fixes
//=============================================================================

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Opc.Ua;
using Opc.Ua.Client;


namespace Siemens.UAClientHelper
{
    public class UAClientHelperAPI
    {
        #region Construction
        public UAClientHelperAPI()
        {
            // Creats the application configuration (containing the certificate) on construction
            mApplicationConfig = CreateClientConfiguration();
        }
        #endregion

        #region Properties
        /// <summary> 
        /// Keeps a session with an UA server.
        /// </summary>
        private Session mSession = null;

        /// <summary> 
        /// Specifies this application.
        /// </summary>
        private ApplicationConfiguration mApplicationConfig = null;

        /// <summary>
        /// Provides the session being established with an OPC UA server.
        /// </summary>
        public Session Session
        {
            get { return mSession; }
        }

        /// <summary>
        /// Provides the event handling for server certificates.
        /// </summary>
        public CertificateValidationEventHandler CertificateValidationNotification = null;

        /// <summary>
        /// Provides the event for value changes of a monitored item.
        /// </summary>
        public MonitoredItemNotificationEventHandler ItemChangedNotification = null;

        /// <summary>
        /// Provides the event for a monitored event item.
        /// </summary>
        public NotificationEventHandler ItemEventNotification = null;

        /// <summary>
        /// Provides the event for KeepAliveNotifications.
        /// </summary>
        public KeepAliveEventHandler KeepAliveNotification;
        #endregion

        #region Discovery
        /// <summary>Finds Servers based on a discovery url</summary>
        /// <param name="discoveryUrl">The discovery url</param>
        /// <returns>ApplicationDescriptionCollection containing found servers</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public ApplicationDescriptionCollection FindServers(string discoveryUrl)
        {
            //Create a URI using the discovery URL
            Uri uri = new Uri(discoveryUrl);
            try
            {
                //Ceate a DiscoveryClient
                DiscoveryClient client = DiscoveryClient.Create(uri);
                //Find servers
                //ApplicationDescriptionCollection servers = client.FindServers(null);
                ApplicationDescriptionCollection servers = client.FindServers(null);
                return servers;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Finds Endpoints based on a server's url</summary>
        /// <param name="discoveryUrl">The server's url</param>
        /// <returns>EndpointDescriptionCollection containing found Endpoints</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public EndpointDescriptionCollection GetEndpoints(string serverUrl)
        {
            //Create a URI using the server's URL
            Uri uri = new Uri(serverUrl);
            try
            {
                //Create a DiscoveryClient
                DiscoveryClient client = DiscoveryClient.Create(uri);
                //Search for available endpoints
                EndpointDescriptionCollection endpoints = client.GetEndpoints(null);
                return endpoints;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Connect/Disconnect
        /// <summary>Establishes the connection to an OPC UA server and creates a session using a server url.</summary>
        /// <param name="url">The Url of the endpoint as string.</param>
        /// <param name="secPolicy">The security policy to use</param>
        /// <param name="msgSecMode">The message security mode to use</param>
        /// <param name="userAuth">Autheticate anonymous or with username and password</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        [Obsolete("Only use if no EndpointDescription of the server's endpoint is available")]
        public void Connect(string url, string secPolicy, MessageSecurityMode msgSecMode, bool userAuth, string userName, string password)
        {
            try
            {
                //Secify application configuration
                ApplicationConfiguration ApplicationConfig = mApplicationConfig;

                //Hook up a validator function for a CertificateValidation event
                mApplicationConfig.CertificateValidator.CertificateValidation += Notificatio_CertificateValidation;

                //Create EndPoint description
                EndpointDescription EndpointDescription = CreateEndpointDescription(url, secPolicy, msgSecMode);

                //Create EndPoint configuration
                EndpointConfiguration EndpointConfiguration = EndpointConfiguration.Create(ApplicationConfig);

                //Create an Endpoint object to connect to server
                ConfiguredEndpoint Endpoint = new ConfiguredEndpoint(null, EndpointDescription, EndpointConfiguration);

                //Create anonymous user identity
                UserIdentity UserIdentity;
                if (userAuth)
                {
                    UserIdentity = new UserIdentity(userName, password);
                }
                else
                {
                    UserIdentity = new UserIdentity();
                }

                //Update certificate store before connection attempt
                ApplicationConfig.CertificateValidator.Update(ApplicationConfig);

                //Create and connect session
                mSession = Session.Create(
                    ApplicationConfig,
                    Endpoint,
                    true,
                    "MySession",
                    60000,
                    UserIdentity,
                    null
                    );

                mSession.KeepAlive += new KeepAliveEventHandler(Notification_KeepAlive);
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Establishes the connection to an OPC UA server and creates a session using an EndpointDescription.</summary>
        /// <param name="endpointDescription">The EndpointDescription of the server's endpoint</param>
        /// <param name="userAuth">Autheticate anonymous or with username and password</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void Connect(EndpointDescription endpointDescription, bool userAuth, string userName, string password)
        {
            try
            {
                //Secify application configuration
                ApplicationConfiguration ApplicationConfig = mApplicationConfig;

                //Hook up a validator function for a CertificateValidation event
                ApplicationConfig.CertificateValidator.CertificateValidation += Notificatio_CertificateValidation;

                //Create EndPoint configuration
                EndpointConfiguration EndpointConfiguration = EndpointConfiguration.Create(ApplicationConfig);

                //Connect to server and get endpoints
                ConfiguredEndpoint mEndpoint = new ConfiguredEndpoint(null, endpointDescription, EndpointConfiguration);

                //Create the binding factory.
                BindingFactory bindingFactory = BindingFactory.Create(mApplicationConfig, ServiceMessageContext.GlobalContext);

                //Creat a session name
                String sessionName = "MySession";

                //Create user identity
                UserIdentity UserIdentity;
                if (userAuth)
                {
                    UserIdentity = new UserIdentity(userName, password);
                }
                else
                {
                    UserIdentity = new UserIdentity();
                }

                //Update certificate store before connection attempt
                ApplicationConfig.CertificateValidator.Update(ApplicationConfig);

                //Create and connect session

                mSession = Session.Create(
                    ApplicationConfig,
                    mEndpoint,
                    true,
                    sessionName,
                    60000,
                    UserIdentity,
                    null
                    );

                mSession.KeepAlive += new KeepAliveEventHandler(Notification_KeepAlive);
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Closes an existing session and disconnects from the server.</summary>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void Disconnect()
        {
            // Close the session.
            try
            {
                mSession.Close(10000);
                mSession.Dispose();
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Namspace
        /// <summary>Returns the namespace uri at the specified index.</summary>
        /// <param name="index">the namespace index</param>
        /// <returns>The namespace uri</returns>
        public String GetNamespaceUri(uint index)
        {
            //Check the length of namespace array
            if (mSession.NamespaceUris.Count > index)
            {   //Get the uri for the namespace index
                return mSession.NamespaceUris.GetString(index);
            }
            else
            {
                Exception e = new Exception("Index is out of range");
                throw e;
            }
        }

        /// <summary>Returns the index of the specified namespace uri.</summary>
        /// <param name="uri">The namespace uri</param>
        /// <returns>The namespace index</returns>
        public uint GetNamespaceIndex(String uri)
        {
            //Get the namespace index of the specified namespace uri
            int namespaceIndex = mSession.NamespaceUris.GetIndex(uri);
            //If the namespace uri doesn't exist, namespace index is -1 
            if (namespaceIndex >= 0)
            {
                return (uint)namespaceIndex;
            }
            else
            {
                Exception e = new Exception("Namespace doesn't exist");
                throw e;
            }
        }

        /// <summary>Returns a list of all namespace uris.</summary>
        /// <returns>The name space array</returns>
        public List<String> GetNamespaceArray()
        {
            List<String> namespaceArray = new List<String>();

            //Read all namespace uris and add the uri to the list
            for (uint i = 0; i < mSession.NamespaceUris.Count; i++)
            {
                namespaceArray.Add(mSession.NamespaceUris.GetString(i));
            }

            return namespaceArray;
        }
        #endregion

        #region Browse
        /// <summary>Browses the root folder of an OPC UA server.</summary>
        /// <returns>ReferenceDescriptionCollection of found nodes</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public ReferenceDescriptionCollection BrowseRoot()
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            try
            {
                //Browse the RootFolder for variables, objects and methods
                mSession.Browse(null, null, ObjectIds.RootFolder, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out continuationPoint, out referenceDescriptionCollection);
                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Browses a node ID provided by a ReferenceDescription</summary>
        /// <param name="refDesc">The ReferenceDescription</param>
        /// <returns>ReferenceDescriptionCollection of found nodes</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public ReferenceDescriptionCollection BrowseNode(ReferenceDescription refDesc)
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescriptionCollection nextreferenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            byte[] revisedContinuationPoint;
            //Create a NodeId using the selected ReferenceDescription as browsing starting point
            NodeId nodeId = ExpandedNodeId.ToNodeId(refDesc.NodeId, null);
            try
            {
                //Browse from starting point for all object types
                mSession.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, 0, out continuationPoint, out referenceDescriptionCollection);

                while (continuationPoint != null)
                {
                    mSession.BrowseNext(null, false, continuationPoint, out revisedContinuationPoint, out nextreferenceDescriptionCollection);
                    referenceDescriptionCollection.AddRange(nextreferenceDescriptionCollection);
                    continuationPoint = revisedContinuationPoint;
                }

                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Browses a node ID provided by a ReferenceDescription</summary>
        /// <param name="refDesc">The ReferenceDescription</param>
        /// <param name="refTypeId">The reference type id</param>
        /// <returns>ReferenceDescriptionCollection of found nodes</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public ReferenceDescriptionCollection BrowseNodeByReferenceType(ReferenceDescription refDesc, NodeId refTypeId)
        {
            //Create a collection for the browse results
            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescriptionCollection nextreferenceDescriptionCollection;
            //Create a continuationPoint
            byte[] continuationPoint;
            byte[] revisedContinuationPoint;
            //Create a NodeId using the selected ReferenceDescription as browsing starting point
            NodeId nodeId = ExpandedNodeId.ToNodeId(refDesc.NodeId, null);
            try
            {
                //Browse from starting point for all object types
                mSession.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, refTypeId, true, 0, out continuationPoint, out referenceDescriptionCollection);

                while (continuationPoint != null)
                {
                    mSession.BrowseNext(null, false, continuationPoint, out revisedContinuationPoint, out nextreferenceDescriptionCollection);
                    referenceDescriptionCollection.AddRange(nextreferenceDescriptionCollection);
                    continuationPoint = revisedContinuationPoint;
                }

                return referenceDescriptionCollection;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Subscription
        /// <summary>Creats a Subscription object to a server</summary>
        /// <param name="publishingInterval">The publishing interval</param>
        /// <returns>Subscription</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public Subscription Subscribe(int publishingInterval)
        {
            //Create a Subscription object
            Subscription subscription = new Subscription(mSession.DefaultSubscription);
            //Enable publishing
            subscription.PublishingEnabled = true;
            //Set the publishing interval
            subscription.PublishingInterval = publishingInterval;
            //Add the subscription to the session
            mSession.AddSubscription(subscription);
            try
            {
                //Create/Activate the subscription
                subscription.Create();
                return subscription;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Ads a monitored item to an existing subscription</summary>
        /// <param name="subscription">The subscription</param>
        /// <param name="nodeIdString">The node Id as string</param>
        /// <param name="itemName">The name of the item to add</param>
        /// <param name="samplingInterval">The sampling interval</param>
        /// <returns>The added item</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public MonitoredItem AddMonitoredItem(Subscription subscription, string nodeIdString, string itemName, int samplingInterval)
        {
            //Create a monitored item
            MonitoredItem monitoredItem = new MonitoredItem();
            //Set the name of the item for assigning items and values later on; make sure item names differ
            monitoredItem.DisplayName = itemName;
            //Set the NodeId of the item
            monitoredItem.StartNodeId = nodeIdString;
            //Set the attribute Id (value here)
            monitoredItem.AttributeId = Attributes.Value;
            //Set reporting mode
            monitoredItem.MonitoringMode = MonitoringMode.Reporting;
            //Set the sampling interval (1 = fastest possible)
            monitoredItem.SamplingInterval = samplingInterval;
            //Set the queue size
            monitoredItem.QueueSize = 1;
            //Discard the oldest item after new one has been received
            monitoredItem.DiscardOldest = true;
            //Define event handler for this item and then add to monitoredItem
            monitoredItem.Notification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);
            try
            {
                //Add the item to the subscription
                subscription.AddItem(monitoredItem);
                //Apply changes to the subscription
                subscription.ApplyChanges();
                return monitoredItem;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Ads a monitored event item to an existing subscription</summary>
        /// <param name="subscription">The subscription</param>
        /// <param name="nodeIdString">The node Id as string</param>
        /// <param name="itemName">The name of the item to add</param>
        /// <param name="samplingInterval">The sampling interval</param>
        /// <param name="filter">The event filter</param>
        /// <returns>The added item</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public MonitoredItem AddEventMonitoredItem(Subscription subscription, string nodeIdString, string itemName, int samplingInterval, EventFilter filter)
        {
            //Create a monitored item
            MonitoredItem monitoredItem = new MonitoredItem(subscription.DefaultItem);
            //Set the name of the item for assigning items and values later on; make sure item names differ
            monitoredItem.DisplayName = itemName;
            //Set the NodeId of the item
            monitoredItem.StartNodeId = nodeIdString;
            //Set the attribute Id (value here)
            monitoredItem.AttributeId = Attributes.EventNotifier;
            //Set reporting mode
            monitoredItem.MonitoringMode = MonitoringMode.Reporting;
            //Set the sampling interval (1 = fastest possible)
            monitoredItem.SamplingInterval = samplingInterval;
            //Set the queue size
            monitoredItem.QueueSize = 1;
            //Discard the oldest item after new one has been received
            monitoredItem.DiscardOldest = true;
            //Set the filter for the event item
            monitoredItem.Filter = filter;

            //Define event handler for this item and then add to monitoredItem
            Session.Notification += new NotificationEventHandler(Notification_MonitoredEventItem);

            try
            {
                //Add the item to the subscription
                subscription.AddItem(monitoredItem);
                //Apply changes to the subscription
                subscription.ApplyChanges();
                return monitoredItem;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Removs a monitored item from an existing subscription</summary>
        /// <param name="subscription">The subscription</param>
        /// <param name="monitoredItem">The item</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public MonitoredItem RemoveMonitoredItem(Subscription subscription, MonitoredItem monitoredItem)
        {
            try
            {
                //Add the item to the subscription
                subscription.RemoveItem(monitoredItem);
                //Apply changes to the subscription
                subscription.ApplyChanges();
                return null;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Removes an existing Subscription</summary>
        /// <param name="subscription">The subscription</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void RemoveSubscription(Subscription subscription)
        {
            try
            {
                //Delete the subscription and all items submitted
                subscription.Delete(true);
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Read/Write
        /// <summary>Reads a node by node Id</summary>
        /// <param name="nodeIdString">The node Id as string</param>
        /// <returns>The read node</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public Node ReadNode(String nodeIdString)
        {
            //Create a nodeId using the identifier string
            NodeId nodeId = new NodeId(nodeIdString);
            //Create a node
            Node node = new Node();
            try
            {
                //Read the dataValue
                node = mSession.ReadNode(nodeId);
                return node;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Reads values from node Ids</summary>
        /// <param name="nodeIdStrings">The node Ids as strings</param>
        /// <returns>The read values as strings</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public List<string> ReadValues(List<String> nodeIdStrings)
        {
            List<NodeId> nodeIds = new List<NodeId>();
            List<Type> types = new List<Type>();
            List<object> values = new List<object>();
            List<ServiceResult> serviceResults = new List<ServiceResult>();
            foreach (string str in nodeIdStrings)
            {
                //Create a nodeId using the identifier string and add to list
                nodeIds.Add(new NodeId(str));
                //No need for types
                types.Add(null);
            }
            try
            {
                //Read the dataValues
                mSession.ReadValues(nodeIds, types, out values, out serviceResults);
                //check ServiceResults to 
                foreach (ServiceResult svResult in serviceResults)
                {
                    if (svResult.ToString() != "Good")
                    {
                        Exception e = new Exception(svResult.ToString());
                        throw e;
                    }
                }
                List<string> resultStrings = new List<string>();
                foreach (object result in values)
                {
                    if (result != null)
                    {
                        if (result.ToString() == "System.Byte[]")
                        {
                            string str = "";
                            str = BitConverter.ToString((byte[])result).Replace("-", ";");
                            resultStrings.Add(str);
                        }
                        if (result.ToString() == "System.String[]")
                        {
                            string str = "";
                            str = String.Join(";", (string[])result);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.Boolean[]")
                        {
                            string str = "";
                            foreach (Boolean intVar in (Boolean[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.Int16[]")
                        {
                            string str = "";
                            foreach (Int16 intVar in (Int16[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.UInt16[]")
                        {
                            string str = "";
                            foreach (UInt16 intVar in (UInt16[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.Int64[]")
                        {
                            string str = "";
                            foreach (Int64 intVar in (Int64[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.Single[]")
                        {
                            string str = "";
                            foreach (float intVar in (float[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else if (result.ToString() == "System.Double[]")
                        {
                            string str = "";
                            foreach (double intVar in (double[])result)
                            {
                                str = str + ";" + intVar.ToString();
                            }
                            str = str.Remove(0, 1);
                            resultStrings.Add(str);
                        }
                        else
                        {
                            resultStrings.Add(result.ToString());
                        }
                    }
                    else
                    {
                        resultStrings.Add("(null)");
                    }
                }
                return resultStrings;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Writes values to node Ids</summary>
        /// <param name="value">The values as strings</param>
        /// <param name="nodeIdString">The node Ids as strings</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void WriteValues(List<String> values, List<String> nodeIdStrings)
        {
            //Create a collection of values to write
            WriteValueCollection valuesToWrite = new WriteValueCollection();
            //Create a collection for StatusCodes
            StatusCodeCollection result = new StatusCodeCollection();
            //Create a collection for DiagnosticInfos
            DiagnosticInfoCollection diagnostics = new DiagnosticInfoCollection();

            foreach (String str in nodeIdStrings)
            {
                //Create a nodeId
                NodeId nodeId = new NodeId(str);
                //Create a dataValue
                DataValue dataValue = new DataValue();
                //Read the dataValue
                try
                {
                    dataValue = mSession.ReadValue(nodeId);
                }
                catch (Exception e)
                {
                    //handle Exception here
                    throw e;
                }

                string test = dataValue.Value.GetType().Name;
                //Get the data type of the read dataValue
                //Handle Arrays here: TBD
                Variant variant = 0;
                try
                {
                    variant = new Variant(Convert.ChangeType(values[nodeIdStrings.IndexOf(str)], dataValue.Value.GetType()));
                }
                catch //no base data type
                {
                    //Handle different arrays types here: TBD
                    if (dataValue.Value.GetType().Name == "string[]")
                    {
                        string[] arrString = values[nodeIdStrings.IndexOf(str)].Split(';');
                        variant = new Variant(arrString);
                    }
                    else if (dataValue.Value.GetType().Name == "Byte[]")
                    {
                        string[] arrString = values[nodeIdStrings.IndexOf(str)].Split(';');
                        Byte[] arrInt = new Byte[arrString.Length];

                        for (int i = 0; i < arrString.Length; i++)
                        {
                            arrInt[i] = Convert.ToByte(arrString[i]);
                        }
                        variant = new Variant(arrInt);
                    }
                    else if (dataValue.Value.GetType().Name == "Int16[]")
                    {
                        string[] arrString = values[nodeIdStrings.IndexOf(str)].Split(';');
                        Int16[] arrInt = new Int16[arrString.Length];

                        for (int i = 0; i < arrString.Length; i++)
                        {
                            arrInt[i] = Convert.ToInt16(arrString[i]);
                        }
                        variant = new Variant(arrInt);
                    }
                }

                //Overwrite the dataValue with a new constructor using read dataType
                dataValue = new DataValue(variant);

                //Create a WriteValue using the NodeId, dataValue and attributeType
                WriteValue valueToWrite = new WriteValue();
                valueToWrite.Value = dataValue;
                valueToWrite.NodeId = nodeId;
                valueToWrite.AttributeId = Attributes.Value;

                //Add the dataValues to the collection
                valuesToWrite.Add(valueToWrite);
            }

            try
            {
                //Write the collection to the server
                mSession.Write(null, valuesToWrite, out result, out diagnostics);
                foreach (StatusCode code in result)
                {
                    if (code != 0)
                    {
                        Exception ex = new Exception(code.ToString());
                        throw ex;
                    }
                }
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Read/Write Struct/UDT
        /// <summary>Reads a struct or UDT by node Id</summary>
        /// <param name="nodeIdString">The node Id as strings</param>
        /// <returns>The read struct/UDT elements as a list of string[3]; string[0] = tag name, string[1] = value, string[2] = opc data type</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public List<string[]> ReadStructUdt(String nodeIdString)
        {
            //Define result list to return var name and var value
            List<string[]> resultStringList = new List<string[]>();

            //Get the type dictionary of desired struct/UDT and the name of desired var to parse
            String parseString;
            String xmlString = GetTypeDictionary(nodeIdString, mSession, out parseString);

            //Parse xmlString to create objects of the struct/UDT containing var name and var data type
            List<object> varList = new List<object>();
            varList = ParseTypeDictionary(xmlString, parseString);

            //Read the struct
            List<NodeId> nodeIds = new List<NodeId>();
            NodeId myNode = new NodeId(nodeIdString);
            nodeIds.Add(myNode);
            List<ServiceResult> serviceResults = new List<ServiceResult>();
            List<Object> values = new List<object>();
            List<Type> types = new List<Type>();
            types.Add(null);
            try
            {
                mSession.ReadValues(nodeIds, types, out values, out serviceResults);
            }
            catch (Exception e)
            {
                //Handle Exception here
                throw e;
            }

            //Check result codes
            foreach (ServiceResult svResult in serviceResults)
            {
                if (svResult.ToString() != "Good")
                {
                    Exception e = new Exception(svResult.ToString());
                    throw e;
                }
            }

            //Create an empty byte-array to store ExtensionObject.Body (containing the whole binary data of the desired strucht/UDT) into
            byte[] readBinaryData = null;
            foreach (object val in values)
            {
                //Cast object to ExtensionObject
                ExtensionObject encodeable = val as ExtensionObject;

                //If encodable == null there might be an array
                if (encodeable == null)
                {
                    ExtensionObject[] exObjArr = val as ExtensionObject[];

                    for (int i = 0; i < exObjArr.Length; i++)
                    {
                        encodeable = exObjArr[i];

                        //Write the body of the ExtensionObject into the byte-array
                        readBinaryData = (byte[])encodeable.Body;

                        //Check for data types and parse byte array 
                        resultStringList.Add(new string[] { "[" + (i + 1).ToString() + "]", "..", "ARRAY[" + exObjArr.Length.ToString() + "] OF STRUCT/UDT" });
                        resultStringList.AddRange(ParseDataToTagsFromDictionary(varList, readBinaryData));
                    }
                }
                else
                {
                    //Write the body(=data) of the ExtensionObject into the byte-array
                    readBinaryData = (byte[])encodeable.Body;

                    //Check for data types and parse byte array 
                    resultStringList.AddRange(ParseDataToTagsFromDictionary(varList, readBinaryData));
                }
            }

            //return result as List<string[]> (string[0]=tag name; string[1]=tag value; string[2]=tag data type
            return resultStringList;
        }

        /// <summary>Writes data to a struct or UDT by node Id</summary>
        /// <param name="nodeIdString">The node Id as strings</param>
        /// <param name="dataToWrite">The data to write as string[3]; string[0] = tag name, string[1] = value, string[2] = opc data type</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void WriteStructUdt(String nodeIdString, List<string[]> dataToWrite)
        {
            //Create a NodeId from the NodeIdString
            NodeId nodeId = new NodeId(nodeIdString);

            //Creat a WriteValueColelction
            WriteValueCollection valuesToWrite = new WriteValueCollection();

            //Create a WriteValue
            WriteValue writevalue = new WriteValue();

            //Create a StatusCodeCollection
            StatusCodeCollection results = new StatusCodeCollection();

            //Create a DiagnosticInfoCollection
            DiagnosticInfoCollection diag = new DiagnosticInfoCollection();

            //Create an ExtensionObject from the Structure given to this function
            ExtensionObject writeExtObj = new ExtensionObject();

            DataValue dataValue = null;

            //This is for regular Struct/UDT            
            if (!dataToWrite[0][2].Contains("OF STRUCT/UDT"))
            {
                //Determine lentgh of byte array needed to contain all data
                Int64 length = GetLengthOfDataToWrite(dataToWrite);

                //Create a byte array
                byte[] bytesToWrite;

                //Parse dataToWrite to the byte array
                bytesToWrite = ParseDataToByteArray(dataToWrite, length);

                //Copy data to extension object body
                writeExtObj.Body = bytesToWrite;

                //Turn the created ExtensionObject into a DataValue
                dataValue = new DataValue(writeExtObj);
            }
            //This is for array of Struct/UDT
            else
            {
                //Get array dimension
                int startDim = dataToWrite[0][2].IndexOf("[") + 1;
                int endDim = dataToWrite[0][2].IndexOf("]");

                string arrDimString = dataToWrite[0][2].Substring(startDim, endDim - startDim);
                Int32 arrDim = Convert.ToInt32(arrDimString);

                //Declar array of extension objects
                ExtensionObject[] exObjArr = new ExtensionObject[arrDim];

                //Split string list containing data to write 
                int index = 0;
                for (int i = 0; i < arrDim; i++)
                {
                    //Create temporary string list
                    List<string[]> splitData = new List<string[]>();

                    //Search for index pos
                    for (int j = index; j < dataToWrite.Count; j++)
                    {
                        if (!dataToWrite[j][0].Contains("[" + (i + 2).ToString() + "]"))
                        {
                            splitData.Add(dataToWrite[j]);
                        }
                        else
                        {
                            index = j + 1;
                            break;
                        }
                    }

                    //Determine lentgh of byte array needed to contain all data
                    Int64 length = GetLengthOfDataToWrite(splitData);

                    //Create a byte array
                    byte[] bytesToWrite;

                    //Parse dataToWrite to the byte array
                    bytesToWrite = ParseDataToByteArray(splitData, length);

                    //Copy data to extension object body
                    exObjArr[i] = new ExtensionObject();
                    exObjArr[i].Body = (Object)bytesToWrite;
                }

                //Turn the created ExtensionObject into a DataValue
                dataValue = new DataValue(exObjArr);
            }

            //Setup for the WriteValue
            writevalue.NodeId = nodeId;
            writevalue.Value = dataValue;
            writevalue.AttributeId = Attributes.Value;
            //Add the created value to the collection
            valuesToWrite.Add(writevalue);

            try
            {
                mSession.Write(null, valuesToWrite, out results, out diag);
            }
            catch (Exception e)
            {
                //Handle Exception here
                throw e;
            }

            //Check result codes
            foreach (StatusCode result in results)
            {
                if (result.ToString() != "Good")
                {
                    Exception e = new Exception(result.ToString());
                    throw e;
                }
            }
        }
        #endregion

        #region Register/Unregister nodes Ids
        /// <summary>Registers Node Ids to the server</summary>
        /// <param name="nodeIdStrings">The node Ids as strings</param>
        /// <returns>The registered Node Ids as strings</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public List<string> RegisterNodeIds(List<String> nodeIdStrings)
        {
            NodeIdCollection nodesToRegister = new NodeIdCollection();
            NodeIdCollection registeredNodes = new NodeIdCollection();
            List<string> registeredNodeIdStrings = new List<string>();
            foreach (string str in nodeIdStrings)
            {
                //Create a nodeId using the identifier string and add to list
                nodesToRegister.Add(new NodeId(str));
            }
            try
            {
                //Register nodes
                mSession.RegisterNodes(null, nodesToRegister, out registeredNodes);

                foreach (NodeId nodeId in registeredNodes)
                {
                    registeredNodeIdStrings.Add(nodeId.ToString());
                }

                return registeredNodeIdStrings;
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Unregister Node Ids to the server</summary>
        /// <param name="nodeIdStrings">The node Ids as string</param>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public void UnregisterNodeIds(List<String> nodeIdStrings)
        {
            NodeIdCollection nodesToUnregister = new NodeIdCollection();
            List<string> registeredNodeIdStrings = new List<string>();
            foreach (string str in nodeIdStrings)
            {
                //Create a nodeId using the identifier string and add to list
                nodesToUnregister.Add(new NodeId(str));
            }
            try
            {
                //Register nodes                
                mSession.UnregisterNodes(null, nodesToUnregister);
            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }
        #endregion

        #region Methods
        /// <summary>Get information about a method's input and output arguments</summary>
        /// <param name="nodeIdString">The node Id of a method as strings</param>
        /// <returns>Argument informations as strings</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public List<string> GetMethodArguments(String nodeIdString)
        {
            //Return input argument node informations
            //Argument[0] = argument type (input or output); 
            //Argument[1] = argument name
            //Argument[2] = argument value
            //Argument[3] = argument data type
            List<string> arguments = new List<string>();

            //Create node id object by node id string
            NodeId nodeId = new NodeId(nodeIdString);

            try
            {
                //Check if node is method
                Node methodNode = ReadNode(nodeIdString);

                if (methodNode.NodeClass == NodeClass.Method)
                {
                    //We need to browse for property (input and output arguments)
                    //Create a collection for the browse results
                    ReferenceDescriptionCollection referenceDescriptionCollection;
                    ReferenceDescriptionCollection nextreferenceDescriptionCollection;
                    //Create a continuationPoint
                    byte[] continuationPoint;
                    byte[] revisedContinuationPoint;

                    //Start browsing

                    //Browse from starting point for properties (input and output)
                    mSession.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, ReferenceTypeIds.HasProperty, true, 0, out continuationPoint, out referenceDescriptionCollection);

                    while (continuationPoint != null)
                    {
                        mSession.BrowseNext(null, false, continuationPoint, out revisedContinuationPoint, out nextreferenceDescriptionCollection);
                        referenceDescriptionCollection.AddRange(nextreferenceDescriptionCollection);
                        continuationPoint = revisedContinuationPoint;
                    }

                    //Check if arguments exist
                    if (referenceDescriptionCollection != null & referenceDescriptionCollection.Count > 0)
                    {
                        foreach (ReferenceDescription refDesc in referenceDescriptionCollection)
                        {
                            if (refDesc.DisplayName.Text == "InputArguments" || refDesc.DisplayName.Text == "OutputArguments" && refDesc.NodeClass == NodeClass.Variable)
                            {
                                List<NodeId> nodeIds = new List<NodeId>();
                                List<Type> types = new List<Type>();
                                List<object> values = new List<object>();
                                List<ServiceResult> serviceResults = new List<ServiceResult>();

                                nodeIds.Add(new NodeId(refDesc.NodeId.ToString()));
                                types.Add(null);

                                //Read the input/output arguments
                                mSession.ReadValues(nodeIds, types, out values, out serviceResults);

                                foreach (ServiceResult svResult in serviceResults)
                                {
                                    if (svResult.ToString() != "Good")
                                    {
                                        Exception e = new Exception(svResult.ToString());
                                        throw e;
                                    }
                                }

                                //Extract arguments
                                foreach (object result in values)
                                {
                                    if (result != null)
                                    {
                                        //Cast object to ExtensionObject because input and output arguments are always extension objects
                                        ExtensionObject encodeable = result as ExtensionObject;
                                        if (encodeable == null)
                                        {
                                            ExtensionObject[] exObjArr = result as ExtensionObject[];
                                            foreach (ExtensionObject exOb in exObjArr)
                                            {
                                                Argument arg = exOb.Body as Argument;
                                                string[] argumentInfos = new string[4];
                                                // Set type: input or output
                                                argumentInfos[0] = refDesc.DisplayName.Text;
                                                // Set argument name
                                                argumentInfos[1] = arg.Name;
                                                // Set argument value
                                                if (arg.Value != null)
                                                {
                                                    argumentInfos[2] = arg.Value.ToString();

                                                    //You might have to cast the value appropriate
                                                    //TBD
                                                }
                                                else
                                                {
                                                    argumentInfos[2] = "";
                                                }

                                                //Set argument data type (no array)
                                                if (arg.ArrayDimensions.Count == 0)
                                                {
                                                    Node node = ReadNode(arg.DataType.ToString());
                                                    argumentInfos[3] = node.DisplayName.ToString();
                                                }
                                                // Data type is array
                                                else if (arg.ArrayDimensions.Count == 1)
                                                {
                                                    Node node = ReadNode(arg.DataType.ToString());
                                                    argumentInfos[3] = node.DisplayName.ToString() + "[" + arg.ArrayDimensions[0].ToString() + "]";
                                                }

                                                arguments.Add(String.Join(";", argumentInfos));
                                            }
                                        }
                                        else
                                        {
                                            arguments.Add(encodeable.ToString());
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return arguments;
                    }

                    return arguments;
                }
                else
                {
                    //Not method; return null
                    return null;
                }

            }
            catch (Exception e)
            {
                //handle Exception here
                throw e;
            }
        }

        /// <summary>Calls a method</summary>
        /// <param name="methodIdString">The node Id as strings</param>
        /// <param name="objectIdString">The object Id as strings</param>
        /// <param name="inputData">The input argument data</param>
        /// <returns>The list of output arguments</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        public IList<object> CallMethod(String methodIdString, String objectIdString, List<string[]> inputData)
        {
            //For calling a method we need it's node id and it's parent object's node id
            NodeId methodNodeId = new NodeId(methodIdString);
            NodeId objectNodeId = new NodeId(objectIdString);

            //Declare an array of objects for the method's input arguments
            Object[] inputArguments = new object[inputData.Count];

            //Parse data types first
            //TBD: arrays for all types

            Boolean parseCheck = false;
            for (int i = 0; i < inputData.Count; i++)
            {
                if (inputData[i][1] == "SByte")
                {
                    SByte value = 0;
                    parseCheck = SByte.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Byte")
                {
                    Byte value = 0;
                    parseCheck = Byte.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Int16")
                {
                    Int16 value = 0;
                    parseCheck = Int16.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1].Contains("Int16["))
                {
                    int pFrom = inputData[i][1].IndexOf("[") + 1;
                    int pTo = inputData[i][1].LastIndexOf("]");
                    string tempString = inputData[i][1].Substring(pFrom, pTo - pFrom);
                    Int16[] value = new Int16[Int16.Parse(tempString)];

                    string[] tempArr = inputData[i][0].Split(';');

                    for (int j = 0; j < tempArr.Length; j++)
                    {
                        parseCheck = Int16.TryParse(tempArr[j], out value[j]);
                    }

                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Int32")
                {
                    Int32 value = 0;
                    parseCheck = Int32.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Int64")
                {
                    Int64 value = 0;
                    parseCheck = Int64.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Boolean")
                {
                    Boolean value = false;
                    parseCheck = Boolean.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "String")
                {
                    inputArguments[i] = inputData[i][0];
                }
                else if (inputData[i][1] == "Float")
                {
                    float value = 0;
                    parseCheck = float.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "DateTime")
                {
                    DateTime value = new DateTime();
                    parseCheck = DateTime.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Double")
                {
                    Double value = 0;
                    parseCheck = Double.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "UInt16")
                {
                    UInt16 value = 0;
                    parseCheck = UInt16.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "UInt32")
                {
                    UInt32 value = 0;
                    parseCheck = UInt32.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "UInt64")
                {
                    UInt64 value = 0;
                    parseCheck = UInt64.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else if (inputData[i][1] == "Double")
                {
                    Double value = 0;
                    parseCheck = Double.TryParse(inputData[i][0], out value);
                    inputArguments[i] = value;
                }
                else
                {
                    Exception e = new Exception("Data type is too complex to be parsed.");
                    throw e;
                }

                if (!parseCheck)
                {
                    Exception e = new Exception("Please check input value");
                    throw e;
                }
            }

            //Declare a list of objects for the method's output arguments
            IList<object> outputArguments = new List<object>();

            //Call the method
            outputArguments = mSession.Call(objectNodeId, methodNodeId, inputArguments);

            return outputArguments;
        }
        #endregion

        #region EventHandling
        /// <summary>Eventhandler to validate the server certificate forwards this event</summary>
        private void Notificatio_CertificateValidation(CertificateValidator certificate, CertificateValidationEventArgs e)
        {
            CertificateValidationNotification(certificate, e);
        }

        /// <summary>Eventhandler for MonitoredItemNotifications forwards this event</summary>
        private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            ItemChangedNotification(monitoredItem, e);
        }

        /// <summary>Eventhandler for MonitoredItemNotifications for event items forwards this event</summary>
        private void Notification_MonitoredEventItem(Session session, NotificationEventArgs e)
        {
            NotificationMessage message = e.NotificationMessage;

            // Check if there is Data available, else return
            if (message.NotificationData.Count == 0)
            {
                return;
            }

            ItemEventNotification(session, e);
        }

        /// <summary>Eventhandler for KeepAlive forwards this event</summary>
        private void Notification_KeepAlive(Session session, KeepAliveEventArgs e)
        {
            KeepAliveNotification(session, e);
        }
        #endregion

        #region Private methods
        /// <summary>Creats a minimal required ApplicationConfiguration</summary>
        /// <param name="localIpAddress">The ip address of the interface to connect with</param>
        /// <returns>The ApplicationConfiguration</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static ApplicationConfiguration CreateClientConfiguration()
        {
            // The application configuration can be loaded from any file.
            // ApplicationConfiguration.Load() method loads configuration by looking up a file path in the App.config.
            // This approach allows applications to share configuration files and to update them.
            // This example creates a minimum ApplicationConfiguration using its default constructor.
            ApplicationConfiguration configuration = new ApplicationConfiguration();

            // Step 1 - Specify the client identity.
            configuration.ApplicationName = "UA Client 1500";
            configuration.ApplicationType = ApplicationType.Client;
            configuration.ApplicationUri = "urn:MyClient"; //Kepp this syntax
            configuration.ProductUri = "SiemensAG.IndustryOnlineSupport";

            // Step 2 - Specify the client's application instance certificate.
            // Application instance certificates must be placed in a windows certficate store because that is 
            // the best way to protect the private key. Certificates in a store are identified with 4 parameters:
            // StoreLocation, StoreName, SubjectName and Thumbprint.
            // When using StoreType = Directory you need to have the opc.ua.certificategenerator.exe installed on your machine

            configuration.SecurityConfiguration = new SecurityConfiguration();
            configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier();
            configuration.SecurityConfiguration.ApplicationCertificate.StoreType = CertificateStoreType.Windows;
            configuration.SecurityConfiguration.ApplicationCertificate.StorePath = "CurrentUser\\My";
            configuration.SecurityConfiguration.ApplicationCertificate.SubjectName = configuration.ApplicationName;

            // Define trusted root store for server certificate checks
            configuration.SecurityConfiguration.TrustedIssuerCertificates.StoreType = CertificateStoreType.Windows;
            configuration.SecurityConfiguration.TrustedIssuerCertificates.StorePath = "CurrentUser\\Root";
            configuration.SecurityConfiguration.TrustedPeerCertificates.StoreType = CertificateStoreType.Windows;
            configuration.SecurityConfiguration.TrustedPeerCertificates.StorePath = "CurrentUser\\Root";

            // find the client certificate in the store.
            X509Certificate2 clientCertificate = configuration.SecurityConfiguration.ApplicationCertificate.Find(true);

            // create a new self signed certificate if not found.
            if (clientCertificate == null)
            {
                // Get local interface ip addresses and DNS name
                List<string> localIps = GetLocalIpAddressAndDns();

                UInt16 keySize = 2048; //must be multiples of 1024
                UInt16 lifeTime = 24; //in month
                UInt16 algorithm = 1; //0 = SHA1; 1 = SHA256

                // this code would normally be called as part of the installer - called here to illustrate.
                // create a new certificate an place it in the current user certificate store.
                clientCertificate = CertificateFactory.CreateCertificate( 
                    configuration.SecurityConfiguration.ApplicationCertificate.StoreType,
                    configuration.SecurityConfiguration.ApplicationCertificate.StorePath,
                    configuration.ApplicationUri,
                    configuration.ApplicationName,
                    null,
                    localIps,
                    keySize,
                    lifeTime
                    );
            }

            // Step 3 - Specify the supported transport quotas.
            // The transport quotas are used to set limits on the contents of messages and are
            // used to protect against DOS attacks and rogue clients. They should be set to
            // reasonable values.
            configuration.TransportQuotas = new TransportQuotas();
            configuration.TransportQuotas.OperationTimeout = 360000;
            configuration.TransportQuotas.MaxStringLength = 67108864;
            configuration.TransportQuotas.MaxByteStringLength = 16777216; //Needed, i.e. for large TypeDictionarys


            // Step 4 - Specify the client specific configuration.
            configuration.ClientConfiguration = new ClientConfiguration();
            configuration.ClientConfiguration.DefaultSessionTimeout = 360000;

            // Step 5 - Validate the configuration.
            // This step checks if the configuration is consistent and assigns a few internal variables
            // that are used by the SDK. This is called automatically if the configuration is loaded from
            // a file using the ApplicationConfiguration.Load() method.
            configuration.Validate(ApplicationType.Client);

            return configuration;
        }

        /// <summary>Creats an EndpointDescription</summary>
        /// <param name="url">The endpoint url</param>
        /// <param name="security">Use security or not</param>
        /// <returns>The EndpointDescription</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static EndpointDescription CreateEndpointDescription(string url, string secPolicy, MessageSecurityMode msgSecMode)
        {
            // create the endpoint description.
            EndpointDescription endpointDescription = new EndpointDescription();

            // submit the url of the endopoint
            endpointDescription.EndpointUrl = url;

            // specify the security policy to use.

            endpointDescription.SecurityPolicyUri = secPolicy;
            endpointDescription.SecurityMode = msgSecMode;

            // specify the transport profile.
            endpointDescription.TransportProfileUri = Profiles.UaTcpTransport;

            return endpointDescription;
        }

        /// <summary>Gets the local IP addresses and the DNS name</summary>
        /// <returns>The list of IPs and names</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static List<string> GetLocalIpAddressAndDns()
        {
            List<string> localIps = new List<string>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIps.Add(ip.ToString());
                }
            }
            if (localIps.Count == 0)
            {
                throw new Exception("Local IP Address Not Found!");
            }
            localIps.Add(Dns.GetHostName());
            return localIps;
        }

        /// <summary>Parses a XML string for the a default data type</summary>
        /// <param name="xmlStringToParse">The XML string containing data type information</param>
        /// <param name="stringToParserFor">The data type as string to search for</param>
        /// <returns>The created objects after parsing for default data type</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static List<object> ParseTypeDictionary(String xmlStringToParse, String stringToParserFor)
        {
            List<object> varList = new List<object>();

            //Remove last XML sign and create a XML document out of the dictionary string
            if (xmlStringToParse.EndsWith("\n"))
            {
                xmlStringToParse = xmlStringToParse.Remove(xmlStringToParse.Length - 1);
            }
            XmlDocument docToParse = new XmlDocument();
            docToParse.LoadXml(xmlStringToParse);

            //Get a XML node list of objectes named by "stringToParseFor"
            docToParse.GetElementsByTagName(stringToParserFor);
            XmlNodeList nodeList;
            nodeList = docToParse.GetElementsByTagName("opc:StructuredType");

            XmlNode foundNode = null;

            //search for the attribute name == "stringToParseFor"
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["Name"].Value == stringToParserFor)
                {
                    foundNode = node;
                    break;
                }
            }

            //check if attribute name was found
            if (foundNode == null)
            {
                return null;
            }

            //get child nodes of parent node with attribute name == "stringToParseFor" and parse for var name and var type
            foreach (XmlNode node in foundNode.ChildNodes)
            {
                string[] dataReferenceStringArray = new string[2];

                dataReferenceStringArray[0] = node.Attributes["Name"].Value;
                dataReferenceStringArray[1] = node.Attributes["TypeName"].Value;
                if (!dataReferenceStringArray[1].Contains("tns:"))
                {
                    dataReferenceStringArray[1] = dataReferenceStringArray[1].Remove(0, 4);
                }
                else
                {
                    dataReferenceStringArray[1] = dataReferenceStringArray[1].Remove(0, 4);
                    dataReferenceStringArray[1] = dataReferenceStringArray[1].Insert(0, "STRUCT/UDT:");
                }


                varList.Add(dataReferenceStringArray);
            }

            //Check if result contains another struct/UDT inside and parse for var name and var type
            //Note: This check is consistent even if there are more structs/UDTs inside of structs/UDTs
            for (int count = 0; count < varList.Count; count++)
            {
                Object varObject = varList[count];
                //"tns:" indicates another strucht/UDT
                if (((string[])varObject)[1].Contains("STRUCT/UDT:"))
                {

                    XmlNode innerNode = null;
                    foreach (XmlNode anotherNode in nodeList)
                    {
                        if (anotherNode.Attributes["Name"].Value == ((string[])varObject)[1].Remove(0, 11))
                        {
                            innerNode = anotherNode;
                            break;
                        }
                    }

                    if (innerNode == null)
                    {
                        return null;
                    }

                    int i = 0;
                    foreach (XmlNode innerChildNode in innerNode.ChildNodes)
                    {
                        string[] innerDataReferenceStringArray = new string[2];
                        innerDataReferenceStringArray[0] = innerChildNode.Attributes["Name"].Value; ;
                        innerDataReferenceStringArray[1] = innerChildNode.Attributes["TypeName"].Value;
                        if (!innerDataReferenceStringArray[1].Contains("tns:"))
                        {
                            innerDataReferenceStringArray[1] = innerDataReferenceStringArray[1].Remove(0, 4);
                        }
                        else
                        {
                            innerDataReferenceStringArray[1] = innerDataReferenceStringArray[1].Remove(0, 4);
                            innerDataReferenceStringArray[1] = innerDataReferenceStringArray[1].Insert(0, "STRUCT/UDT:");
                        }

                        varList.Insert(varList.IndexOf(varObject) + 1 + i, innerDataReferenceStringArray);
                        i += 1;

                        if (i == innerNode.ChildNodes.Count)
                        {
                            string[] innerDataReferenceStringArrayEnd = new string[2];
                            innerDataReferenceStringArrayEnd[0] = ((string[])varObject)[0];
                            innerDataReferenceStringArrayEnd[1] = "END_STRUCT/UDT";
                            varList.Insert(varList.IndexOf(varObject) + 1 + i, innerDataReferenceStringArrayEnd);
                        }
                    }
                }
            }
            return varList;
        }

        /// <summary>Parses a byte array to objects containing tag names and tag data types</summary>
        /// <param name="varList">List of object containing tag names and tag data types</param>
        /// <param name="byteResult">A byte array to parse</param>
        /// <returns>A list of string[3]; string[0] = tag name, string[1] = value, string[2] = opc data type</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static List<string[]> ParseDataToTagsFromDictionary(List<Object> varList, byte[] byteResult)
        {
            //Define result list to return var name, var value and var data type
            List<string[]> resultStringList = new List<string[]>();

            //Byte decoding index
            int index = 0;

            //Int used to decode arrays
            Int32 arraylength = 0;

            //Start decoding for opc data types
            foreach (object val in varList)
            {
                string[] dataReferenceStringArray = new string[3];
                dataReferenceStringArray[0] = ((string[])val)[0];

                if (((string[])val)[1] == "Boolean" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToBoolean(byteResult, index).ToString();
                    index += 1;
                }
                else if (((string[])val)[1] == "Int16" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToInt16(byteResult, index).ToString();
                    index += 2;
                }
                else if (((string[])val)[1] == "Int16" && arraylength > 0)
                {
                    Int16[] tempArray = new Int16[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToInt16(byteResult, index);
                        index += 2;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "UInt16" && arraylength > 0)
                {
                    UInt16[] tempArray = new UInt16[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToUInt16(byteResult, index);
                        index += 2;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "Int32" && !(arraylength > 0) && !((string[])val)[0].Contains("_Size"))
                {
                    dataReferenceStringArray[1] = BitConverter.ToInt32(byteResult, index).ToString();
                    index += 4;
                }
                else if (((string[])val)[1] == "Int32" && arraylength > 0)
                {
                    Int32[] tempArray = new Int32[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToInt32(byteResult, index);
                        index += 4;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "UInt32" && arraylength > 0)
                {
                    UInt32[] tempArray = new UInt32[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToUInt32(byteResult, index);
                        index += 4;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "Int64" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToInt64(byteResult, index).ToString();
                    index += 8;
                }
                else if (((string[])val)[1] == "UInt64" && arraylength > 0)
                {
                    UInt64[] tempArray = new UInt64[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToUInt64(byteResult, index);
                        index += 8;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "Float" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToSingle(byteResult, index).ToString();
                    index += 4;
                }
                else if (((string[])val)[1] == "Float" && arraylength > 0)
                {
                    Single[] tempArray = new Single[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToSingle(byteResult, index);
                        index += 4;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "Double" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToDouble(byteResult, index).ToString();
                    index += 8;
                }
                else if (((string[])val)[1] == "Double" && arraylength > 0)
                {
                    Double[] tempArray = new Double[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToDouble(byteResult, index);
                        index += 8;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "String" && !(arraylength > 0))
                {
                    Int32 stringlength = BitConverter.ToInt32(byteResult, index);
                    index += 4;
                    if (stringlength > 0)
                    {
                        dataReferenceStringArray[1] = Encoding.UTF8.GetString(byteResult, index, stringlength);
                        index += stringlength;
                    }
                    else
                    {
                        dataReferenceStringArray[1] = "";
                    }
                }
                else if (((string[])val)[1] == "CharArray")
                {
                    Int32 stringlength = BitConverter.ToInt32(byteResult, index);
                    index += 4;
                    if (stringlength > 0)
                    {
                        dataReferenceStringArray[1] = Encoding.UTF8.GetString(byteResult, index, stringlength);
                        index += stringlength;
                    }
                    else
                    {
                        dataReferenceStringArray[1] = "";
                    }
                }
                else if (((string[])val)[1] == "UInt16" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToUInt16(byteResult, index).ToString();
                    index += 2;
                }
                else if (((string[])val)[1] == "UInt32" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToUInt32(byteResult, index).ToString();
                    index += 4;
                }
                else if (((string[])val)[1] == "Int64" && arraylength > 0)
                {
                    Int64[] tempArray = new Int64[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = BitConverter.ToInt64(byteResult, index);
                        index += 8;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "UInt64" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = BitConverter.ToUInt64(byteResult, index).ToString();
                    index += 8;
                }
                else if (((string[])val)[1] == "Byte" && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = byteResult[index].ToString();
                    index += 1;
                }
                else if (((string[])val)[1] == "Int32" && ((string[])val)[0].Contains("_Size"))
                {
                    arraylength = BitConverter.ToInt32(byteResult, index);
                    dataReferenceStringArray[1] = arraylength.ToString();
                    index += 4;
                }
                else if (((string[])val)[1] == "Byte" && arraylength > 0)
                {
                    Int32[] tempArray = new Int32[arraylength];
                    for (int i = 0; i < arraylength; i++)
                    {
                        tempArray[i] = byteResult[index];
                        index += 1;
                    }
                    dataReferenceStringArray[1] = String.Join(";", tempArray);
                    dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                    arraylength = 0;
                }
                else if (((string[])val)[1] == "String" && arraylength > 0)
                {
                    for (int i = 0; i < arraylength; i++)
                    {
                        Int32 stringlength = BitConverter.ToInt32(byteResult, index);
                        index += 4;
                        if (stringlength > 0)
                        {

                            dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], Encoding.UTF8.GetString(byteResult, index, stringlength));
                            dataReferenceStringArray[1] = String.Concat(dataReferenceStringArray[1], ";");
                            index += stringlength;
                        }
                        else
                        {
                            dataReferenceStringArray[1] = dataReferenceStringArray[1] + ";";
                        }
                    }
                    arraylength = 0;
                }
                else if (((string[])val)[1].Contains("STRUCT/UDT") && !(arraylength > 0))
                {
                    dataReferenceStringArray[1] = "..";
                }
                else
                {
                    Exception e = new Exception("Data type is too complex to be parsed." + System.Environment.NewLine + "Please avoid array of UDT/Struct inside UDT/Struct");
                    throw e;
                }
                dataReferenceStringArray[2] = ((string[])val)[1];
                resultStringList.Add(dataReferenceStringArray);
            }
            return resultStringList;
        }

        /// <summary>Parses a byte array to objects containing tag names and tag data types</summary>
        /// <param name="dataToWrite">The data to analyze</param>
        /// <returns>The length</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static Int64 GetLengthOfDataToWrite(List<string[]> dataToWrite)
        {
            Int64 length = 0;
            Int32 arraySize = 0;
            foreach (string[] val in dataToWrite)
            {
                if (arraySize > 0)
                {
                    arraySize = 0;
                    continue;
                }
                if (val[2] == "Boolean")
                {
                    length += 1;
                }
                else if (val[2] == "Int16")
                {
                    length += 2;
                }
                else if (val[2] == "Float")
                {
                    length += 4;
                }
                else if (val[2] == "Double")
                {
                    length += 8;
                }
                else if (val[2] == "UInt16")
                {
                    length += 2;
                }
                else if (val[2] == "Int32" && !(val[0].Contains("_Size")))
                {
                    length += 4;
                }
                else if (val[2] == "UInt32")
                {
                    length += 4;
                }
                else if (val[2] == "Int64")
                {
                    length += 8;
                }
                else if (val[2] == "UInt64")
                {
                    length += 8;
                }
                else if (val[2] == "Byte")
                {
                    length += 1;
                }
                else if (val[2] == "String")
                {
                    length += val[1].Length + 4;
                }
                else if (val[2] == "CharArray")
                {
                    length += val[1].Length + 4;
                }
                else if (val[2] == "Int32" && val[0].Contains("_Size"))
                {
                    arraySize = Convert.ToInt32(val[1]);

                    if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("String"))
                    {
                        string[] tempStringArr = new string[arraySize];
                        tempStringArr = ((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[1].Split(';');
                        for (int ii = 0; ii < arraySize; ii++)
                        {
                            length += (4 + tempStringArr[ii].Length);
                        }
                        length += 4;
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Int16"))
                    {
                        length += (4 + (arraySize * 2));
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Int32"))
                    {
                        length += (4 + (arraySize * 4));
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Int64"))
                    {
                        length += (4 + (arraySize * 8));
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Boolean"))
                    {
                        length += (4 + arraySize);
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Byte"))
                    {
                        length += (4 + arraySize);
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Float"))
                    {
                        length += (4 + (arraySize * 4));
                    }
                    else if (((string[])dataToWrite[dataToWrite.IndexOf(val) + 1])[2].Contains("Int32"))
                    {
                        length += (4 + (arraySize * 8));
                    }
                }
                else if (val[2].Contains("STRUCT/UDT"))
                {
                    ;
                }
                else
                {
                    Exception e = new Exception("Unknow data type. Can't determine length of data");
                    throw e;
                }
            }
            return length;
        }

        /// <summary>Browses for the desired type dictonary to parse for containing data types</summary>
        /// <param name="nodeIdString">The node Id string</param>
        /// <param name="theSessionToBrowseIn">The current session to browse in</param>
        /// <param name="parseString">The name of the var to parse for inside of dictionary</param>
        /// <returns>The dictionary as ASCII string</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static String GetTypeDictionary(String nodeIdString, Session theSessionToBrowseIn, out String parseString)
        {
            //Read the desired node first and chekc if it's a variable
            Node node = theSessionToBrowseIn.ReadNode(nodeIdString);
            if (node.NodeClass == NodeClass.Variable)
            {
                //Get the node id of node's data type
                VariableNode variableNode = (VariableNode)node.DataLock;
                NodeId nodeId = new NodeId(variableNode.DataType.Identifier, variableNode.DataType.NamespaceIndex);

                //Browse for HasEncoding
                ReferenceDescriptionCollection refDescCol;
                byte[] continuationPoint;
                theSessionToBrowseIn.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, ReferenceTypeIds.HasEncoding, true, 0, out continuationPoint, out refDescCol);

                //Check For found reference
                if (refDescCol.Count == 0)
                {
                    Exception ex = new Exception("No data type to encode. Could be a build-in data type you want to read.");
                    throw ex;
                }

                //Check for HasEncoding reference with name "Default Binary"
                bool dataTypeFound = false;
                foreach (ReferenceDescription refDesc in refDescCol)
                {
                    if (refDesc.DisplayName.Text == "Default Binary")
                    {
                        nodeId = new NodeId(refDesc.NodeId.Identifier, refDesc.NodeId.NamespaceIndex);
                        dataTypeFound = true;
                    }
                    else if (dataTypeFound == false)
                    {
                        Exception ex = new Exception("No default binary data type found.");
                        throw ex;
                    }
                }

                //Browse for HasDescription
                refDescCol = null;
                theSessionToBrowseIn.Browse(null, null, nodeId, 0u, BrowseDirection.Forward, ReferenceTypeIds.HasDescription, true, 0, out continuationPoint, out refDescCol);

                //Check For found reference
                if (refDescCol.Count == 0)
                {
                    Exception ex = new Exception("No data type description found in address space.");
                    throw ex;
                }

                //Read from node id of the found description to get a value to parse for later on
                nodeId = new NodeId(refDescCol[0].NodeId.Identifier, refDescCol[0].NodeId.NamespaceIndex);
                DataValue resultValue = theSessionToBrowseIn.ReadValue(nodeId);
                parseString = resultValue.Value.ToString();

                //Browse for ComponentOf from last browsing result inversly
                refDescCol = null;
                theSessionToBrowseIn.Browse(null, null, nodeId, 0u, BrowseDirection.Inverse, ReferenceTypeIds.HasComponent, true, 0, out continuationPoint, out refDescCol);

                //Check if reference was found
                if (refDescCol.Count == 0)
                {
                    Exception ex = new Exception("Data type isn't a component of parent type in address space. Can't continue decoding.");
                    throw ex;
                }

                //Read from node id of the found HasCompoment reference to get a XML file (as HEX string) containing struct/UDT information

                nodeId = new NodeId(refDescCol[0].NodeId.Identifier, refDescCol[0].NodeId.NamespaceIndex);
                resultValue = theSessionToBrowseIn.ReadValue(nodeId);

                //Convert the HEX string to ASCII string
                String xmlString = ASCIIEncoding.ASCII.GetString((byte[])resultValue.Value);

                //Return the dictionary as ASCII string
                return xmlString;
            }
            {
                Exception ex = new Exception("No variable data type found");
                throw ex;
            }
        }

        /// <summary>Parses data to write to a byte array</summary>
        /// <param name="dataToWrite">The data to write as string[3]; string[0] = tag name, string[1] = value, string[2] = opc data type</param>
        /// <param name="dataLength">The length of the data to write</param>
        /// <returns>The parsed byte array</returns>
        /// <exception cref="Exception">Throws and forwards any exception with short error description.</exception>
        private static byte[] ParseDataToByteArray(List<string[]> dataToWrite, Int64 dataLength)
        {
            byte[] bytesToWrite = new byte[dataLength];
            Int32 convertIndex = 0;
            Int32 arraySize = 0;
            foreach (string[] val in dataToWrite)
            {
                if (val[2] == "Boolean" && !(arraySize > 0))
                {
                    Boolean tempBool = Convert.ToBoolean(val[1]);
                    bytesToWrite[convertIndex] = Convert.ToByte(tempBool);
                    convertIndex += 1;
                }
                else if (val[2] == "Int16" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToInt16(val[1])), 0, bytesToWrite, convertIndex, 2);
                    convertIndex += 2;
                }
                else if (val[2] == "UInt16" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(val[1])), 0, bytesToWrite, convertIndex, 2);
                    convertIndex += 2;
                }
                else if (val[2] == "Float" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToSingle(val[1])), 0, bytesToWrite, convertIndex, 4);
                    convertIndex += 4;
                }
                else if (val[2] == "Double" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToDouble(val[1])), 0, bytesToWrite, convertIndex, 8);
                    convertIndex += 8;
                }
                else if (val[2] == "Int32" && !(val[0].Contains("_Size")) && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(val[1])), 0, bytesToWrite, convertIndex, 4);
                    convertIndex += 4;
                }
                else if (val[2] == "UInt32" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(val[1])), 0, bytesToWrite, convertIndex, 4);
                    convertIndex += 4;
                }
                else if (val[2] == "Int64" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToInt64(val[1])), 0, bytesToWrite, convertIndex, 8);
                    convertIndex += 8;
                }
                else if (val[2] == "UInt64" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt64(val[1])), 0, bytesToWrite, convertIndex, 8);
                    convertIndex += 8;
                }
                else if (val[2] == "Byte" && !(arraySize > 0))
                {
                    bytesToWrite[convertIndex] = Convert.ToByte(val[1]);
                    convertIndex += 1;
                }
                else if (val[2] == "String" && !(arraySize > 0))
                {
                    Array.Copy(BitConverter.GetBytes(val[1].Length), 0, bytesToWrite, convertIndex, 4);
                    convertIndex += 4;
                    foreach (Char c in val[1])
                    {
                        bytesToWrite[convertIndex] = Convert.ToByte(c);
                        convertIndex += 1;
                    }
                }
                else if (val[2] == "CharArray")
                {
                    Array.Copy(BitConverter.GetBytes(val[1].Length), 0, bytesToWrite, convertIndex, 4);
                    convertIndex += 4;
                    foreach (Char c in val[1])
                    {
                        bytesToWrite[convertIndex] = Convert.ToByte(c);
                        convertIndex += 1;
                    }
                }
                else if (val[2] == "Int32" && val[0].Contains("_Size"))
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(val[1])), 0, bytesToWrite, convertIndex, 4);
                    arraySize = Convert.ToInt32(val[1]);
                    convertIndex += 4;
                }
                else if (val[2] == "Byte" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            bytesToWrite[convertIndex] = Convert.ToByte(tempString);
                            convertIndex += 1;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "Int16" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToInt16(tempString)), 0, bytesToWrite, convertIndex, 2);
                            convertIndex += 2;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "UInt16" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(tempString)), 0, bytesToWrite, convertIndex, 2);
                            convertIndex += 2;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "Int32" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToInt32(tempString)), 0, bytesToWrite, convertIndex, 4);
                            convertIndex += 4;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "UInt32" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToUInt32(tempString)), 0, bytesToWrite, convertIndex, 4);
                            convertIndex += 4;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "Int64" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToInt64(tempString)), 0, bytesToWrite, convertIndex, 8);
                            convertIndex += 8;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "UInt64" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToUInt64(tempString)), 0, bytesToWrite, convertIndex, 8);
                            convertIndex += 8;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "Float" && arraySize > 0)
                {
                    String tempString = "";
                    foreach (Char c in val[1])
                    {
                        if (c != ';')
                        {
                            tempString = String.Concat(tempString, c);
                        }
                        else
                        {
                            Array.Copy(BitConverter.GetBytes(Convert.ToSingle(tempString)), 0, bytesToWrite, convertIndex, 4);
                            convertIndex += 4;
                            tempString = "";
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2] == "String" && arraySize > 0)
                {
                    string[] tempStringArr = new string[arraySize];
                    tempStringArr = val[1].Split(';');
                    for (int ii = 0; ii < arraySize; ii++)
                    {
                        Array.Copy(BitConverter.GetBytes(tempStringArr[ii].Length), 0, bytesToWrite, convertIndex, 4);
                        convertIndex += 4;
                        foreach (Char c in tempStringArr[ii])
                        {
                            bytesToWrite[convertIndex] = Convert.ToByte(c);
                            convertIndex += 1;
                        }
                    }
                    arraySize = 0;
                }
                else if (val[2].Contains("STRUCT/UDT"))
                {
                    ;
                }
                else
                {
                    Exception e = new Exception("Can't covert " + val[0] + ".");
                    throw e;
                }
            }
            return bytesToWrite;
        }
        #endregion
    }
}