using UnityEngine;

public class Item
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; set; }
    public string Text { get; private set; }

    public Item(int id, string title, string description, string text)
    {
        Id = id;
        Title = title;
        Description = description;
        Icon = Resources.Load<Sprite>("scroll");
        Text = text;
    }

    public Item(Item item)
    {
        Id = item.Id;
        Title = item.Title;
        Description = item.Description;
        Icon = item.Icon;
        Text = item.Text;
    }
}
