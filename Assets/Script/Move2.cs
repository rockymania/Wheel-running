using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Main;
public class Move : MonoBehaviour {

	//開始轉
	private bool mTurn = false;
	//反彈效果
	private bool mShock = false;

	[SerializeField]
	private GameObject[] mWheel;
	
	[SerializeField]
	private float mRunungSpeed = 2.0f;
	[SerializeField]
	private float mRunToStopSpeed= 1.0f;
	[SerializeField]
	private bool mReceiveResult = false;
	[SerializeField]
	private bool mStartGame = false;

	//狀態
	private byte mStage = 0;
	//跑幾個後停止
	private int mStopCheck = 5;
	//強制矯正
	private int mIsReset = 99;

	//位置
	private float mTopX = 105;
	private Vector3[] SlotPos;

	private int mMaxWheel = 7;

	private string UIImage = "image/";

	private void HandleStartEvent()
	{
		if (mTurn) return;
		mStartGame = false;
		mTurn = true;
		mStopCheck = 5;
		mStage = 0;
	}

	private void HandleStopEvent()
	{
		mReceiveResult = false;
		if (!mTurn) return;
		mStage = 1;
	}



	// Use this for initialization
	void Start()
	{
		Main.Instance.OnStart += HandleStartEvent;
		Main.Instance.OnStop += HandleStopEvent;

		//mWheel = new GameObject[7];

		SlotPos = new Vector3[]
		{
			new Vector3(0, -mTopX * 6, 0.0f),
			new Vector3(0, -mTopX * 5, 0.0f),
			new Vector3(0, -mTopX * 4, 0.0f),
			new Vector3(0, -mTopX * 3, 0.0f),
			new Vector3(0, -mTopX * 2, 0.0f),
			new Vector3(0, -mTopX * 1, 0.0f),
			new Vector3(0, -mTopX * 0, 0.0f),
		};

		for (int i = 0; i < mMaxWheel; i++)
		{
			//mWheel[i] = this.transform.Find(i.ToString()).gameObject;
			mWheel[i].transform.localPosition = SlotPos[i];
			SetPic(0, i);

		}

	}
	
	// Update is called once per frame
	void Update () {

		if(mStartGame)
        {
			mStartGame = false;
			mTurn = true;
			mStopCheck = 5;
			mStage = 0;
		}

		if(mReceiveResult)
        {
			mReceiveResult = false;
			if (!mTurn) return;
			mStage = 1;
		}

		Turning ();
	}

	public IEnumerator Shock()
	{
		if (mTurn)
			yield break;
		if (!mShock)
			yield break;
			
		mShock = false;

		////先呈現過頭
		//this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y + 5);
		//yield return new WaitForSeconds (0.2f);
  //      //反彈
  //      this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 10);
  //      yield return new WaitForSeconds(0.2f);
        ////在過頭輕微
        //this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y+5);
        //yield return new WaitForSeconds (0.2f);
        //反彈
        //this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y-5);
        //yield return new WaitForSeconds (0.2f);


    }

	public void Turning()
	{
		if (!mTurn)
			return;
		
		for (int i = 0; i < mMaxWheel; i++) {
			//一般的模式下-就是順著一定的時間前進。
			if (mStage == 0)
				mWheel [i].transform.localPosition = new Vector3 (mWheel [i].transform.localPosition.x, mWheel [i].transform.localPosition.y - mRunungSpeed);
			else if (mStage == 1) {//收到結果
				mWheel [i].transform.localPosition = new Vector3 (mWheel [i].transform.localPosition.x, mWheel [i].transform.localPosition.y - mRunToStopSpeed);
			} else if (mStage == 2) {
				//要停下來了，呈現跑過頭跟迴轉的呈現
				mTurn = false;
				mShock = true;
			}

			if (mWheel [i].transform.localPosition.y <= (-mTopX * mMaxWheel)) {
				if (mStage == 0) {
					SetPic (mStage,i);
				}

				if (mStage == 1) {
					mStopCheck -= 1;
					if (mStopCheck < 2)
						SetPic (0,i);
					else
						SetPic (mStage,i);
				}

				if (mStopCheck <= 0) {
					mStopCheck = 0;
					mStage = 2;
				}
				mIsReset = i;
			}

		}

		if (mIsReset != 99) {
			//強制矯正
			if (mIsReset == 0) {
				mWheel [0].transform.localPosition = new Vector2 (mWheel [0].transform.localPosition.x, mWheel [6].transform.localPosition.y + mTopX);
			} else {
				mWheel [mIsReset].transform.localPosition = new Vector2 (mWheel [mIsReset].transform.localPosition.x, mWheel [mIsReset - 1].transform.localPosition.y + mTopX);
			}
			mIsReset = 99;
		}

		if (mStage == 2)
			StartCoroutine (Shock ());
	}



	private void SetPic(int vStage, int vKind)
	{
		mWheel[vKind].GetComponent<Image>().sprite = Resources.Load<Sprite>(UIImage + GetPicName(0, Random.Range(0, 10)));
	}

	private string GetPicName(int vStage,int vKind)
	{
		string vString = string.Empty;
		vString = string.Format("{0}",vKind.ToString("D2"));

		return vString;
	}
}
