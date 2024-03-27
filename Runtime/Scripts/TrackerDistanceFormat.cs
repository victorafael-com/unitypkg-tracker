using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using System.Text;
namespace com.victorafael.tracking
{

    public class TrackerDistanceDisplay : MonoBehaviour
    {
        public Tracker tracker;
        public UnityEvent<string> updateText;
        public bool metricSystem = true;
        public bool displayUnit = true;

        private StringBuilder stringBuilder = new StringBuilder();

        void Start()
        {
            if (tracker == null)
            {
                tracker = FindObjectOfType<Tracker>();
                if (tracker == null)
                {
                    Debug.LogError("No tracker found on scene", gameObject);
                    gameObject.SetActive(false);
                    return;
                }
            }
            tracker.events.distanceUpdate.AddListener(UpdateText);
            if (!tracker.events.useDistance)
            {
                tracker.events.useDistance = true;
                Debug.LogWarning($"Enabling useDistance on Tracker {tracker.name} to display distance on {name}.", gameObject);
            }
        }

        private void UpdateText(float distance)
        {
            if (!metricSystem)
            {
                distance = distance * 3.28084f;
            }
            stringBuilder.Clear();

            stringBuilder.Append(distance.ToString("F2"));

            if (displayUnit)
            {
                stringBuilder.Append(metricSystem ? "m" : "ft");
            }
            updateText.Invoke(stringBuilder.ToString());
        }
    }

}