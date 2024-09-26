using UnityEngine;

public class OpenLink : MonoBehaviour
{
    // Đường dẫn URL bạn muốn mở
    private string url = "https://www.aptosfaucet.com/";

    // Hàm này sẽ được gọi khi người dùng nhấp vào đối tượng
    public void OnClick()
    {
        // Mở liên kết trong trình duyệt
        Application.OpenURL(url);
    }
}
