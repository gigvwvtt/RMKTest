namespace Application.Entities;

public class Department
{
    public string Name { get; }
    public string HeadPerson { get; }
    public string ParentDepartment { get; }
    public string Comment { get; }
    public List<Department> ChildDepartments { get; set; }
    public List<Department> upperDepartments { get; set; }

    public Department(string name, string headPerson, string parentDepartment, string comment)
    {
        char[] charsToTrim = {',', '.', ' '};
        Name = name.TrimStart(charsToTrim).TrimEnd(charsToTrim);
        HeadPerson = headPerson.TrimStart(charsToTrim).TrimEnd(charsToTrim);
        ParentDepartment = parentDepartment.TrimStart(charsToTrim).TrimEnd(charsToTrim);
        Comment = comment.TrimStart(charsToTrim).TrimEnd(charsToTrim);
    }

    public Department()
    {
        
    }
}