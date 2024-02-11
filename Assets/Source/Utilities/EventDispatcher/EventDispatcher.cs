using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : GenericSingleton<EventDispatcher>
{
    private Dictionary<EventKey, Action<object>> _listeners = new Dictionary<EventKey, Action<object>>();

    /// <summary>
    /// Register to listen for eventKey
    /// </summary>
    /// <param name="eventKey">EventKey that object want to listen</param>
    /// <param name="callback">Callback will be invoked when this eventKey be raised</para	m>
    public void RegisterListener(EventKey eventKey, Action<object> callback)
    {
        if (callback == null)
        {
            Debug.Log($"EventDispatcher: RegisterListener: event {eventKey}: callback = null !!");
            return;
        }
        if (eventKey == EventKey.None)
        {
            Debug.Log($"EventDispatcher: RegisterListener: event = None !!");
            return;
        }

        if (_listeners.ContainsKey(eventKey))
        {
            _listeners[eventKey] += callback;
        }
        else
        {
            _listeners.Add(eventKey, null);
            _listeners[eventKey] += callback;
        }
    }

    /// <summary>
    /// Posts the event. This will notify all listener that register for this event
    /// </summary>
    /// <param name="eventKey">EventKey.</param>
    /// <param name="sender">Sender, in some case, the Listener will need to know who send this message.</param>
    /// <param name="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
    public void PostEvent(EventKey eventKey, object param = null)
    {
        if (!_listeners.ContainsKey(eventKey))
        {
            Debug.Log($"EventDispatcher: PostEvent: No listeners for this event {eventKey}");
            return;
        }

        var callbacks = _listeners[eventKey];
        if (callbacks != null)
        {
            callbacks(param);
        }
        else
        {
            Debug.Log($"EventDispatcher: PostEvent: Post event {eventKey}, but no listener remain, Remove this key");
            _listeners.Remove(eventKey);
        }
    }

    /// <summary>
    /// Removes the listener. Use to Unregister listener
    /// </summary>
    /// <param name="eventKey">EventKey.</param>
    /// <param name="callback">Callback.</param>
    public void RemoveListener(EventKey eventKey, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventKey))
        {
            _listeners[eventKey] -= callback;
        }
        else
        {
            Debug.Log($"EventDispatcher: RemoveListener, not found key {eventKey}");
        }
    }

    /// <summary>
    /// Clears all the listener.
    /// </summary>
    public void ClearAllListener()
    {
        _listeners.Clear();
    }
}