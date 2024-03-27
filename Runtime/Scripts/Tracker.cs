using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace com.victorafael.tracking
{
    public class Tracker : MonoBehaviour
    {
        [System.Serializable]
        public class Events
        {
            public bool useDistance;
            public UnityEvent<float> distanceUpdate;
            public bool useScreenPosition;
            public UnityEvent<Vector3> screenPositionUpdate;
            public bool useWorldPosition;
            public UnityEvent<Vector3> worldPositionUpdate;
            public UnityEvent<bool> onTrackingStateChange;
        }
        public enum UpdateMode { Update, LateUpdate, FixedUpdate, Manual }
        private static Tracker instance;

        public bool tracking;
        public Transform anchorOverride;
        public new Camera camera;
        public Transform trackedObject;
        protected Vector3 trackedPosition;
        public UpdateMode updateMode = UpdateMode.LateUpdate;
        public Events events;


        // Start is called before the first frame update
        public static void TrackTransform(Transform transform)
        {
            if (instance == null) return;
            instance.StartTracking(transform);
        }
        public static void TrackPosition(Vector3 position)
        {
            if (instance == null) return;
            instance.StartTracking(position);
        }
        public static void Stop()
        {
            if (instance == null) return;
            instance.StopTracking();
            instance.trackedObject = null;
        }

        public virtual void StartTracking(Transform target)
        {
            if (!tracking || trackedObject != target)
            {
                trackedObject = target;
                tracking = true;
                events.onTrackingStateChange?.Invoke(true);
            }
        }
        public virtual void StartTracking(Vector3 position)
        {
            if (!tracking || trackedPosition != position)
            {
                trackedPosition = position;
                tracking = true;
                events.onTrackingStateChange?.Invoke(true);
            }
        }
        public virtual void StopTracking()
        {
            if (tracking)
            {
                trackedObject = null;
                tracking = false;
                events.onTrackingStateChange?.Invoke(false);
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        protected virtual void Start()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            if (tracking && trackedObject != null)
            {
                events.onTrackingStateChange?.Invoke(true);
            }
            else
            {
                events.onTrackingStateChange?.Invoke(false);
                tracking = false;
            }
        }
        public Vector2 ScreenToCanvasPosition(Canvas canvas, Vector2 screenPosition)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out localPoint);

            return localPoint;
        }
        void Update() { UpdateTracker(UpdateMode.Update); }
        void LateUpdate() { UpdateTracker(UpdateMode.LateUpdate); }
        void FixedUpdate() { UpdateTracker(UpdateMode.FixedUpdate); }
        public void UpdateTracking() { UpdateTracker(UpdateMode.Manual); }

        void UpdateTracker(UpdateMode mode)
        {
            if (updateMode != mode || !tracking)
            {
                return;
            }

            if (trackedObject != null)
            {
                trackedPosition = trackedObject.position;
            }

            if (events.useDistance)
            {
                Vector3 from = anchorOverride != null ? anchorOverride.position : camera.transform.position;
                events.distanceUpdate?.Invoke(Vector3.Distance(from, trackedPosition));
            }
            if (events.useScreenPosition)
            {
                var screenPos = camera.WorldToScreenPoint(trackedPosition);
                events.screenPositionUpdate?.Invoke(screenPos);
            }
            if (events.useWorldPosition)
            {
                events.worldPositionUpdate?.Invoke(trackedPosition);
            }
        }
    }
}
