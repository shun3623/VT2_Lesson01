using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    public Vector3 moveInputVelocity = Vector3.zero;    // 移動操作の入力ベクトル
    public Vector3 lookInputVelocity = Vector3.zero;    // カメラ操作の入力ベクトル
    public float moveSpeed = 10f;

    public GameObject topAxis;    // タンクの上部(砲塔)の オブジェクト参照
    public GameObject cannonAxis; // タンク砲の オブジェクト参照

    public GameObject bulletPrefab;     // 弾のプレハブ
    public GameObject shotPoint;        // 弾の発射位置オブジェクト

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        //move.x = moveInputVelocity.x;       // 入力の X 成分を X 成分に指定
        move.z = moveInputVelocity.y;       // 入力の Y 成分を Z 成分に指定

        Vector3 bodyTorque = Vector3.zero;
        bodyTorque.y = moveInputVelocity.x;   // 入力の X 成分を回転の Y 成分に指定

        transform.Translate(move * moveSpeed * Time.deltaTime);
        transform.Rotate(bodyTorque * Time.deltaTime * 90);

        // === タンク上部の回転 ===
        Vector3 topTorque = Vector3.zero;
        topTorque.y = lookInputVelocity.x;  // 入力の X 成分を回転の Y 成分に指定

        topAxis.transform.Rotate(topTorque * Time.deltaTime * 90);

        // === タンク砲の回転 ===
        Vector3 cannonTorque = Vector3.zero;
        cannonTorque.x = lookInputVelocity.y * -1;  // 入力の Y 成分を回転の X 成分に指定

        cannonAxis.transform.Rotate(cannonTorque * Time.deltaTime * 90 );
    }

    void OnMove(InputValue value)
    {
        Debug.Log($"move value is {value.Get()}");

        moveInputVelocity =  value.Get<Vector2>();

    }

    // カメラ操作の入力イベント
    void OnLook(InputValue value)
    {
        Debug.Log($"look value is { value.Get() }");

        lookInputVelocity = value.Get<Vector2>();
    }

    // 攻撃の入力イベント
    void OnAttack(InputValue value)
    {
        Debug.Log($"attack value is {value.Get()}");

        GameObject bullet = Instantiate(
            bulletPrefab,                   // 生成する弾のプレハブ
            shotPoint.transform.position,   // 弾丸の生成位置
            shotPoint.transform.rotation    // 弾丸の生成回転
            );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(shotPoint.transform.forward * 25, ForceMode.Impulse);
    }
}
