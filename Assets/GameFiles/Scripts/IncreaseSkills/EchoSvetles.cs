using UnityEngine;

 public static class EchoSvetles
 {
     private const string Key = "Echo";

     public static int Amount => PlayerPrefs.GetInt(Key, 0);

     public static void Add(int amount)
     {
         PlayerPrefs.SetInt(Key, Amount + amount);
         PlayerPrefs.Save();
     }

     public static bool Spend(int amount)
     {
         if (Amount < amount)
             return false;
         
         PlayerPrefs.SetInt(Key, Amount - amount);
         PlayerPrefs.Save();

         return true;
     }
 }
