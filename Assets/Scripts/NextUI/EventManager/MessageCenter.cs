using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    // Static class to manager all the messages of different UI forms
    public static class MessageCenter
    {
        public delegate void DelMessageExecute(MessageData mData);

        // Dictionary to cach all the messages
        private static Dictionary<string, DelMessageExecute> _messages =
            new Dictionary<string, DelMessageExecute>();

        public static void AddListener(string messageType, DelMessageExecute handle)
        {
            if (!_messages.ContainsKey(messageType))
            {
                _messages.Add(messageType, null);
            }
            _messages[messageType] += handle;
        }

        public static void RemoveListenner(string messageType, DelMessageExecute handle)
        {
            if (_messages.ContainsKey(messageType))
            {
                _messages[messageType] -= handle;
            }
        }

        public static void CLearMessages()
        {
            _messages.Clear();
        }

        public static void ExecuteMessge(string messageType, MessageData mData)
        {
            DelMessageExecute execute;

            if (_messages.TryGetValue(messageType, out execute))
            {
                if (execute != null)
                {
                    execute(mData);
                }
            }
        }

        public static void SendMessage(string messageType, DelMessageExecute handle)
        {
            AddListener(messageType, handle);
        }
    }

    // Strcutrue to store message name and its content
    public class MessageData
    {
        private string _message;
        private Object _data;

        public MessageData(string message, Object data)
        {
            _message = message;
            _data = data;
        }
    }
}
