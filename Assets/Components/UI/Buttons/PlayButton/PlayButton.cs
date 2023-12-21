using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private List<GameEvent> OnPlayEventList;
    [SerializeField] private List<GameEvent> OnPauseEventList;

    private bool isPaused = false;

    public void Play()
    {
        isPaused = false;
        if (icon) icon.sprite = pauseIcon;

        RaiseEventsInList(OnPlayEventList);
    }

    public void PlayWithoutRaising()
    {
        isPaused = false;
        if (icon) icon.sprite = pauseIcon;
    }

    public void Pause()
    {
        isPaused = true;
        if (icon) icon.sprite = playIcon;

        RaiseEventsInList(OnPauseEventList);
    }

    public void TogglePlayPause()
    {
        if (isPaused)
        {
            Play();
        }
        else
        {
            Pause();
        }
    }

    private void RaiseEventsInList(List<GameEvent> listGameEvents)
    {
        foreach (GameEvent e in listGameEvents)
        {
            e.Raise();
        }
    }
}
