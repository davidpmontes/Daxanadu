using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] private GameObject storeBox;

    [SerializeField] private GameObject scrollingBackground;

    [SerializeField] private GameObject dialogueCursorStart;
    [SerializeField] private GameObject storeCursorStart;

    [SerializeField] private TextUtility textUtility;

    private bool started;
    private bool userCanInteract;

    //public delegate void ConversationHandler();
    //public event ConversationHandler Ended;

    //20 characters max per line, excluding > and ^
    private string[] convo = new string[] {
        "Welcome!^-->",
        "Welcome!^-->",
        "How may I help?^-->",
        "Buy or Sell?^NXT",
        "BUY?^BUY"
    };

    private string[] storeList = new string[]
    {
        "Key^-->",
        "Sword^-->",
        "Axe^-->"
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        if (!userCanInteract)
            return;

        if (InputController.Instance.isJumpStart)
        {
            //switch (state)
            //{
            //    case STATES.NORMAL:
            //        break;
            //    case STATES.NEXT:
            //        StopCoroutine(caretBlinkCoroutine);
            //        caretNext.SetActive(false);
            //        state = STATES.NORMAL;
            //        break;
            //    case STATES.FINISH:
            //        HideDialogue();
            //        EndAll();
            //        break;
            //    case STATES.BUY:
            //        HideDialogue();
            //        ShowStoreBox();
            //        break;
            //}
        }

        if (InputController.Instance.isCancel)
        {
            HideDialogue();
        }
    }

    public void ShowStoreBox()
    {
        if (started)
            return;

        started = true;

        PortraitContainer.Instance.Show();
        storeBox.SetActive(true);

        Player.Instance.GetComponent<Player>().Pause();

        scrollingBackground.transform.position = Vector3.zero;

        userCanInteract = false;

        //textUtility.Initialize(STATES.NORMAL, storeCursorStart.transform.position, textUtility.INSTANT_SPEED);
        //textUtility.DrawConversation(storeList);
    }

    public void ShowConversation()
    {
        if (started)
            return;

        started = true;

        PortraitContainer.Instance.Show();
        LandscapeContainer.Instance.Show();
        storeBox.SetActive(false);

        Player.Instance.GetComponent<Player>().Pause();

        scrollingBackground.transform.position = Vector3.zero;

        userCanInteract = true;

        //textUtility.Initialize(STATES.NORMAL, dialogueCursorStart.transform.position, textUtility.SLOW_SPEED);
        //textUtility.DrawConversation(convo);
    }

    public void HideDialogue()
    {
        StopAllCoroutines();
        //caretFinished.SetActive(false);
        //caretNext.SetActive(false);
        PortraitContainer.Instance.Hide();
        LandscapeContainer.Instance.Hide();
        textUtility.RecycleLetters();
        //if (Ended != null) Ended.Invoke();
    }

    private void EndAll()
    {
        Player.Instance.GetComponent<Player>().Unpause();
        started = false;
    }
}
