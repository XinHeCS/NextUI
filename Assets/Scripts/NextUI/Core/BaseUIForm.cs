using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    public class BaseUIForm : Behaviour
    {
        // Current UI type
        // refer to class UIType for more
        // infomation
        private UIType _currrentUIType = new UIType();

        internal UIType CurrrentUIType
        {
            get
            {
                return _currrentUIType;
            }

            set
            {
                _currrentUIType = value;
            }
        }

        // Some basic methods to control the ui element
        public virtual void Display()
        {
            gameObject.SetActive(true);
        }

        // Move this UI node out of the UI stack
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Redisplay()
        {
            gameObject.SetActive(true);
        }

        // Keep current UI node in the UI stack
        // but can't interactive with it
        public virtual void Freeze()
        {
            gameObject.SetActive(true);
        }
    }
}
