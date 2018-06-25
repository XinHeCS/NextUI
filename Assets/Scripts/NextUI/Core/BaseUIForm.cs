using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
