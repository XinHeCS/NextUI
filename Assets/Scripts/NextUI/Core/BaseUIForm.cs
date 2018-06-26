using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace NextUI
{
    public class BaseUIForm : MonoBehaviour
    {
        // Current UI type
        // refer to class UIType for more
        // infomation
        private UIType _currentUIType = new UIType();

        internal UIType CurrentUIType
        {
            get
            {
                return _currentUIType;
            }

            set
            {
                _currentUIType = value;
            }
        }

        #region Four status of ui forms

        // Some basic methods to control the ui element
        public virtual void Display()
        {
            gameObject.SetActive(true);
            if (CurrentUIType.formType == UIFormType.PopUp)
            {
                UIMask.GetInstance().SetMask(this);
            }
        }

        // Move this UI node out of the UI stack
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if (CurrentUIType.formType == UIFormType.PopUp)
            {
                UIMask.GetInstance().CancelMask();
            }
        }

        public virtual void Redisplay()
        {
            gameObject.SetActive(true);
            if (CurrentUIType.formType == UIFormType.PopUp)
            {
                UIMask.GetInstance().SetMask(this);
            }
        }

        // Keep current UI node in the UI stack
        // but can't interactive with it
        public virtual void Freeze()
        {
            gameObject.SetActive(true);
        }
        #endregion


        #region Some useful methods

        /// <summary>
        /// Register events call back functions for child ui forms
        /// </summary>
        /// <param name="uiName">Name of child ui form</param>
        /// <param name="eventType">Event type to regist</param>
        /// <param name="eventCallback">Callback function</param>
        protected void RegisterEvent(
            string uiName, 
            EventTriggerType eventType, 
            UnityAction<BaseEventData> eventCallback)
        {
            var uiForm = transform.Find(uiName).gameObject;
            var trigger = uiForm.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = eventType
            };
            entry.callback.AddListener(eventCallback);

            trigger.triggers.Add(entry);
        }

        protected void OpenForm(string uiName)
        {
            UIManager.GetInstance().ShowUIForm(uiName);
        }

        protected void CloseForm(string uiName)
        {
            UIManager.GetInstance().CloseUIForm(uiName);
        }

        protected void SendMessage(string messageType, MessageCenter.DelMessageExecute handle)
        {
            MessageCenter.SendMessage(messageType, handle);
        }

        protected void ExecuteMessge(string messageType, MessageData mData)
        {
            MessageCenter.ExecuteMessge(messageType, mData);
        }
        #endregion
    }
}
