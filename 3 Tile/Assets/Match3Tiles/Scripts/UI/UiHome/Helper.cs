using UnityEngine;

public static class Helper
{
    public static string FormatCurrency(int amount)
    {
        if (amount >= 1000)
        {
            // Chuy?n ??i thành giá tr? có 'k' và ??nh d?ng v?i 2 ch? s? th?p phân
            float valueInK = amount / 1000f;
            return string.Format("{0:0.##}k", valueInK);
        }
        else
        {
            // Tr? v? giá tr? g?c d??i d?ng chu?i
            return amount.ToString();
        }
    }

    public static Vector3 GetMousePos3D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 EndPos = ray.origin + ray.direction * 36f; // thay doi gia tri Z;

        return EndPos;
    }

}
