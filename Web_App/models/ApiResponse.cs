public class ApiResponse
{
    public List<User> Results { get; set; }
    public Info Info { get; set; }
}

public class Info
{
    public int Page { get; set; }
    public int Results { get; set; }
    public string Seed { get; set; }
    public string Version { get; set; }
}

public class User
{
    public Name Name { get; set; }
    public string Gender { get; set; }
    public Dob Dob { get; set; }
    public string Email { get; set; }
    public Location Location { get; set; }
    public Picture Picture { get; set; }
}

public class Name
{
    public string Title { get; set; }
    public string First { get; set; }
    public string Last { get; set; }

    public string FullName => $"{First} {Last}";
}

public class Dob
{
    public DateTime Date { get; set; }
    public int Age { get; set; }
}

public class Location
{
    public string Country { get; set; }
}

public class Picture
{
    public string Large { get; set; }
    public string Medium { get; set; }
    public string Thumbnail { get; set; }
}
