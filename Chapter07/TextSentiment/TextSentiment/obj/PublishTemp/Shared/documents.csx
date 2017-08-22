public class document
{
	public string id { get; set; }
	public string text { get; set; }
	public double score { get; set; }
}

public class error
{
	public string id { get; set; }
	public string message { get; set; }
}

public class documentList
{
	public List<document> documents { get; set; }
	public List<error> errors { get; set; }
}