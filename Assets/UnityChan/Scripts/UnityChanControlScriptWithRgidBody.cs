//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
// 必要なコンポーネントの列記
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{
        public GameObject empower1;
        public GameObject bomb1;
        public GameObject empower2;
        public GameObject bomb2;
        public GameObject empower3;
        public GameObject bomb3;
        public GameObject hitFire;

        public AudioSource pushSound;
        public AudioSource empowerSound;
        public AudioSource bomb1Sound;
        public AudioSource bomb2Sound;
        public AudioSource bomb3Sound;
        public AudioSource beHurt;



        Rock nowTouchedrock;

        public GUIBarScript hpBar;
        public int maxHP = 8;
        public int hp = 8;
        public bool hpIsLocked = false;

        public float h, v = 0;

		public float animSpeed = 1.5f;				// アニメーション再生速度設定
		public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
		public bool useCurves = true;				// Mecanimでカーブ調整を使うか設定する
		// このスイッチが入っていないとカーブは使われない
		public float useCurvesHeight = 0.5f;		// カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

		// 以下キャラクターコントローラ用パラメタ
		// 前進速度
		public float forwardSpeed = 2.0f;
		// 後退速度
		public float backwardSpeed = 2.0f;
		// 旋回速度
		public float rotateSpeed = 2.0f;
		// ジャンプ威力
		public float jumpPower = 3.0f; 
		// キャラクターコントローラ（カプセルコライダ）の参照
		private CapsuleCollider col;
		private Rigidbody rb;
		// キャラクターコントローラ（カプセルコライダ）の移動量
		private Vector3 velocity;
		// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;							// キャラにアタッチされるアニメーターへの参照
		private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

		private GameObject cameraObject;	// メインカメラへの参照
		
		// アニメーター各ステートへの参照
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");
        static int walkBackState = Animator.StringToHash("Base Layer.WalkBack");
        static int empowerState = Animator.StringToHash("Base Layer.Empower");
        static int attackState = Animator.StringToHash("Base Layer.Attack");
        static int damagedState = Animator.StringToHash("Base Layer.Damaged");

        // 初期化
        void Start ()
		{
			// Animatorコンポーネントを取得する
			anim = GetComponent<Animator> ();
			// CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
			//メインカメラを取得する
			cameraObject = GameObject.FindWithTag ("MainCamera");
			// CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}
	
	
		// 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
		void FixedUpdate ()
		{
            //Xbox joystick A button
            if (Input.GetKeyDown("joystick button 0"))
            {
                InitPressedTime();
            }
            if (Input.GetKey("joystick button 0"))
            {
                SetEmpower(true);
                EmpowerPressedTime();
                
            }
            if (Input.GetKeyUp("joystick button 0"))
            {
                SetEmpower(false);
            }


            if (Input.GetKey("joystick button 1"))
            {
                SetPull(true);
            }

            if (Input.GetKeyUp("joystick button 1"))
            {
                SetPull(false);
            }
#if UNITY_EDITOR
            h = Input.GetAxis("Horizontal");                // 入力デバイスの水平軸をhで定義
            v = Input.GetAxis("Vertical");              // 入力デバイスの垂直軸をvで定義
#endif
            anim.SetFloat ("Speed", Mathf.Abs(v) > Mathf.Abs(h) ? Mathf.Abs(v) : Mathf.Abs(h));							// Animator側で設定している"Speed"パラメタにvを渡す
			//anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
			anim.speed = animSpeed;								// Animatorのモーション再生速度に animSpeedを設定する
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
			rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする


            if (currentBaseState.nameHash == idleState || currentBaseState.nameHash == locoState || currentBaseState.nameHash == jumpState || currentBaseState.nameHash == walkBackState || currentBaseState.nameHash == restState)
            {

                Vector3 moveVectorX = cameraObject.transform.right * h;
                Vector3 moveVectorY = cameraObject.transform.up * v;

                Vector3 moveVector = (moveVectorX + moveVectorY).normalized * Time.deltaTime;

                Vector3 cubeRotDir = new Vector3(moveVector.x, 0, moveVector.y).normalized;
                transform.LookAt(transform.position + cubeRotDir);


                //transform.LookAt(transform.position + new Vector3(h, 0, v));
                // 以下、キャラクターの移動処理
                velocity = new Vector3(cubeRotDir.x, 0, cubeRotDir.z) * 0.7f; //new Vector3(h, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                        // キャラクターのローカル空間での方向に変換
                //velocity = transform.TransformDirection(velocity);

                velocity *= forwardSpeed;

                ////以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
                //if (v > 0.1)
                //{
                //    velocity *= forwardSpeed;       // 移動速度を掛ける
                //}
                //else if (v < -0.1)
                //{
                //    velocity *= backwardSpeed;  // 移動速度を掛ける
                //}

                if (Input.GetButtonDown("Jump"))
                {   // スペースキーを入力したら

                    //アニメーションのステートがLocomotionの最中のみジャンプできる
                    if (currentBaseState.nameHash == locoState)
                    {
                        //ステート遷移中でなかったらジャンプできる
                        if (!anim.IsInTransition(0))
                        {
                            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                            anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                        }
                    }
                }


                // 上下のキー入力でキャラクターを移動させる
                //transform.localPosition += velocity * Time.fixedDeltaTime;
                rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);

                // 左右のキー入力でキャラクタをY軸で旋回させる
                //transform.Rotate(0, h * rotateSpeed, 0);

            }
			// 以下、Animatorの各ステート中での処理
			// Locomotion中
			// 現在のベースレイヤーがlocoStateの時
			if (currentBaseState.nameHash == locoState) {
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves) {
					resetCollider ();
				}
			}
		// JUMP中の処理
		// 現在のベースレイヤーがjumpStateの時
		else if (currentBaseState.nameHash == jumpState) {
				//cameraObject.SendMessage ("setCameraPositionJumpView");	// ジャンプ中のカメラに変更
				// ステートがトランジション中でない場合
				if (!anim.IsInTransition (0)) {
				
					// 以下、カーブ調整をする場合の処理
					if (useCurves) {
						// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
						// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
						// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
						float jumpHeight = anim.GetFloat ("JumpHeight");
						float gravityControl = anim.GetFloat ("GravityControl"); 
						if (gravityControl > 0)
							rb.useGravity = false;	//ジャンプ中の重力の影響を切る
										
						// レイキャストをキャラクターのセンターから落とす
						Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit ();
						// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
						if (Physics.Raycast (ray, out hitInfo)) {
							if (hitInfo.distance > useCurvesHeight) {
								col.height = orgColHight - jumpHeight;			// 調整されたコライダーの高さ
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3 (0, adjCenterY, 0);	// 調整されたコライダーのセンター
							} else {
								// 閾値よりも低い時には初期値に戻す（念のため）					
								resetCollider ();
							}
						}
					}
					// Jump bool値をリセットする（ループしないようにする）				
					anim.SetBool ("Jump", false);
				}
			}
		// IDLE中の処理
		// 現在のベースレイヤーがidleStateの時
		else if (currentBaseState.nameHash == idleState) {
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves) {
					resetCollider ();
				}
				// スペースキーを入力したらRest状態になる
				if (Input.GetButtonDown ("Jump")) {
					anim.SetBool ("Rest", true);
				}
			}
		// REST中の処理
		// 現在のベースレイヤーがrestStateの時
		else if (currentBaseState.nameHash == restState) {
				//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
				// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
				if (!anim.IsInTransition (0)) {
					anim.SetBool ("Rest", false);
				}
			}
		}

		//void OnGUI ()
		//{
		//	GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "Interaction");
		//	GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
		//	GUI.Label (new Rect (Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
		//	GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
		//	GUI.Label (new Rect (Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
		//	GUI.Label (new Rect (Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
		//	GUI.Label (new Rect (Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		//}


		// キャラクターのコライダーサイズのリセット関数
		void resetCollider ()
		{
			// コンポーネントのHeight、Centerの初期値を戻す
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}

        public void SetAxis(Vector2 v2)
        {
            h = v2.x;
            v = v2.y;
            DebugLogger.Log("("  + v2.x + "," + v2.y + ")");
        }

        public void ClearAxis()
        {
            h = v = 0;
        }
        bool flag1, flag2, flag3, flag4;

        GameObject b3;
        public void SetEmpower(bool value)
        {
            anim.SetBool("Empower", value);
            if (value == false)
            {
                empowerSound.Stop();
                Destroy(b3);
                Instantiate(hitFire, transform.position, Quaternion.identity);
                if (isTouchingRock)
                {
                    pushSound.Play();
                    if(flag3)
                        nowTouchedrock.Push(transform.position.x, transform.position.z, 4);
                    else if(flag2)
                        nowTouchedrock.Push(transform.position.x, transform.position.z, 3);
                    else if (flag1)
                        nowTouchedrock.Push(transform.position.x, transform.position.z, 2);
                    else
                        nowTouchedrock.Push(transform.position.x, transform.position.z);
                }
            }
        }

        public void SetPull(bool value)
        {
            anim.SetBool("Pull", value);
            if(value == true && isTouchingRock)
            {
                nowTouchedrock.Pull(transform.position.x, transform.position.z);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Enemy" && !hpIsLocked)
            {
                anim.SetTrigger("Damage");
                beHurt.Play();
                hp -= 1;
                hpBar.Value = (float)hp / maxHP;
                StartCoroutine(SetPlayerToUnTouchable());
                if(hp <= 0 )
                {
                    col.enabled = false;
                    anim.SetTrigger("Dead");
                    EventManager.GameOver();
                    StartCoroutine(ReloadLevelDelay(1.8f));
                }
            }
        }

        bool isTouchingRock = false;
        void OnTriggerStay(Collider collider)
        {
            if(collider.tag == "Rock")
            {
                isTouchingRock = true;
                nowTouchedrock = collider.GetComponent<Rock>();
            }
        }

        IEnumerator ReloadLevelDelay(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Application.LoadLevel(0);
        }

        IEnumerator SetPlayerToUnTouchable()
        {
            hpIsLocked = true;
            yield return new WaitForSeconds(1.5f);
            hpIsLocked = false;
        }
        
        public float t = 0;
        public void InitPressedTime()
        {
            t = 0;
            flag1 = flag2 = flag3 = flag4 = false;
            GameObject em1 = Instantiate(empower1, transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
            empowerSound.Play();
            Destroy(em1, 1);
        }

        public void EmpowerPressedTime()
        {
            t += Time.deltaTime;
            DebugLogger.Log(t);
            if (t >= 1 && t < 2 && flag1 == false)
            {
                flag1 = true;
                GameObject b1 = Instantiate(bomb1, transform.position, Quaternion.identity) as GameObject;
                Destroy(b1, 1);
                bomb1Sound.Play();
                GameObject em2 =  Instantiate(empower2, transform.position + new Vector3(0, 1f, 0) +transform.right * 0.1f, Quaternion.identity) as GameObject;
                em2.transform.localScale *= 4;
                Destroy(em2, 1);
            }
            else if (t >= 2 && t < 3 && flag2 == false)
            {
                flag2 = true;
                GameObject b2 = Instantiate(bomb2, transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
                Destroy(b2, 1);
                bomb2Sound.Play();
                GameObject em3 = Instantiate(empower3, transform.position + new Vector3(0, 1f, 0) + transform.right * 0.1f, Quaternion.identity) as GameObject;
                Destroy(em3, 2.5f);
            }
            else if (t >= 3.5f && flag3 == false)
            {
                flag3 = true;
                b3 = Instantiate(bomb3, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity) as GameObject;
                bomb3Sound.Play();
            }
        }
    }
}