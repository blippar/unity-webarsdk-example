using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;


//refers to the culture related information like Language, country/region, formats
//Main purpose to solve the number format from different language like folat is not same for English and Italin. 
public class Globalization : MonoBehaviour
{
    private void Awake()
    {
        CultureInfo culturInfo = new CultureInfo("en-US");
        CultureInfo.CurrentCulture = culturInfo;
    }
}
