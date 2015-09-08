using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UniRx;

public class RankingContent : MonoBehaviour
{
	[SerializeField]
	private Text nickName;
	[SerializeField]
	private Image photo;
	[SerializeField]
	private Text record;
	[SerializeField]
	private Text rank;

	private UserRecord userRecord;

	public void Init(UserRecord record)
	{
		this.userRecord = record;
		InitializeUI ();
		SetUserImage ();

		if (User.GetInstance.UserID == record.userID) {
			ChangeTextColorToRed();
		}
	}

	private void ChangeTextColorToRed()
	{
		nickName.color = Color.red;
		record.color = Color.red;
		rank.color = Color.red;
	}

	private void SetUserImage()
	{
		if (userRecord.userType == User.LoginType.Facebook) {
			var pictureURL = string.Format ("http://graph.facebook.com/{0}/picture?type=square", userRecord.userID);
			ObservableWWW.GetWWW (pictureURL).Subscribe (www => {
				photo.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
			});
		}
	}

	private void InitializeUI()
	{
		nickName.text = userRecord.nickName;
		rank.text = userRecord.rank.ToString ();

		TimeSpan time = TimeSpan.FromSeconds (userRecord.record);
		record.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 10);
	}
}
