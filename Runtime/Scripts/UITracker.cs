using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.victorafael.tracking
{
    [RequireComponent(typeof(RectTransform))]
    public class UITracker : Tracker
    {
        private RectTransform m_rectTransform;
        private RectTransform m_parent;
        public GameObject trackingRoot;
        protected override void Start()
        {
            if (!transform.parent || !trackingRoot)
            {
                gameObject.SetActive(false);
            }

            events.onTrackingStateChange.AddListener(OnTrackingStateChange);
            events.screenPositionUpdate.AddListener(UpdatePosition);
            m_rectTransform = GetComponent<RectTransform>();
            m_rectTransform.anchorMin = Vector2.zero;
            m_rectTransform.anchorMax = Vector2.zero;
            m_parent = m_rectTransform.parent.GetComponent<RectTransform>();
            events.useScreenPosition = true;

            base.Start();
        }

        private void OnTrackingStateChange(bool tracking)
        {
            trackingRoot.SetActive(tracking);
        }

        private void UpdatePosition(Vector3 screenPos)
        {
            if (screenPos.z < 0) screenPos *= -1;

            Vector2 anchorPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

            m_rectTransform.anchoredPosition = anchorPos * m_parent.sizeDelta;
        }
    }
}