using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.Id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.Title == itemName);
    }

    void BuildDatabase()
    {
        items = new List<Item>()
        {
            new Item(0, "Pocz¹tek cz.1", "04:00", "jakis tekst 2")
        ,
            new Item(1, "Pocz¹tek cz.2", "04:30", "jakis tekst fab")
        ,
            new Item(2, "Pocz¹tek cz.3", "05:00", "jakis tekst 3")
        ,
            new Item(3, "Znaleziona notatka", "06:00", "jakis tekst 4")
        ,
            new Item(4, "Znaleziona notatka", "06:30", "jakis tekst 5")
        ,
            new Item(5, "Znaleziona notatka", "07:00", "jakis tekst 6")
        ,
            new Item(6, "Znaleziona notatka", "07:15", "jakis tekst 7")
        ,
            new Item(7, "Znaleziona notatka", "07:35", "jakis tekst 8")
        ,
            new Item(8, "Znaleziona notatka", "07:45", "jakis tekst 9")
        ,
            new Item(9, "Znaleziona notatka", "08:00", "jakis tekst 10")
        ,
            new Item(10, "Znaleziona notatka", "08:30", "jakis tekst 11")
        ,
            new Item(11, "Znaleziona notatka", "08:50", "jakis tekst 12")
        };
    }
}
