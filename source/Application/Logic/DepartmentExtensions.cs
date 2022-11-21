using System.Text;
using Application.Entities;

namespace Application.Logic;

public class DepartmentExtensions
{
    static List<string> excelData = ReadData.ReadExcel("Структура.xlsx");
    static List<Department> departments = CreateDepartments(excelData);

    /// <summary>
    /// Вся информация о департаменте
    /// </summary>
    public static string GetAllInfo(string departmentName)
    {
        var sb = new StringBuilder();
        sb.AppendLine(GetHead(departmentName));
        sb.AppendLine(GetKeyExecutives(departmentName));
        sb.AppendLine(GetVicePresident(departmentName));
        sb.AppendLine(GetExecutiveUnderVicePresident(departmentName));
        return sb.ToString();
    }

    public static string GetHead(string departmentName)
    {
        var department = new Department();
        try
        {
            department = GetDepartmentByName(departmentName);
        }
        catch (Exception)
        {
        }

        return department.HeadPerson.Length == 0
            ? "Нет руководителя"
            : $"Руководитель отдела - {department.HeadPerson}";
    }

    public static string GetKeyExecutives(string departmentName)
    {
        var department = new Department();
        try
        {
            department = GetDepartmentByName(departmentName);
        }
        catch (Exception)
        {
        }

        var list = new List<Department>();
        while (department.upperDepartments.Count != 0)
        {
            if (department.Name != departmentName.ToLower())
                list.Add(department);
            department = departments.First(x => x.Name == department.ParentDepartment);
        }

        var keyExecutives =
            list.Aggregate(new StringBuilder(), (builder, dep) => builder.Append($"{dep.HeadPerson}, "));
        return $"Ключевые руководители отдела - {keyExecutives.Remove(keyExecutives.Length-2, 2)}";
    }

    public static string GetVicePresident(string departmentName)
    {
        var department = new Department();
        try
        {
            department = GetDepartmentByName(departmentName);
        }
        catch (Exception)
        {
        }

        while (department.ParentDepartment.Length != 0)
        {
            department = departments.First(x => x.Name == department.ParentDepartment);
        }

        return department.HeadPerson.Length == 0
            ? "Нет Вице-президента"
            : $"Вице-президент отдела - {department.HeadPerson}";
    }


    public static string GetExecutiveUnderVicePresident(string departmentName)
    {
        var department = new Department();
        try
        {
            department = GetDepartmentByName(departmentName);
        }
        catch (Exception)
        {
        }

        Department lastDepartment = null;

        while (department.ParentDepartment.Length != 0)
        {
            lastDepartment = department;
            department = departments.First(x => x.Name == department.ParentDepartment);
        }

        return lastDepartment.HeadPerson.Length == 0
            ? "Нет руководителя под Вице-президентом"
            : $"Руководитель под вице-президентом - {lastDepartment.HeadPerson}";
    }

    public static Department GetDepartmentByName(string departmentName)
    {
        try
        {
            return departments.First(x => x.Name == departmentName.ToLower());
        }
        catch (Exception)
        {
            Console.WriteLine("Не найден отдел!");
            return null;
        }
    }

    public static List<Department> CreateDepartments(List<string> excelData)
    {
        var list = new List<Department>();
        foreach (var row in excelData)
        {
            var splitted = row.Split('\t');
            if (list.Any(x=>x.Name==splitted[1]))
                Console.WriteLine("Такой руководитель уже существует!");
            else
            {
                list.Add(new Department(
                    splitted[1].ToLower(),
                    splitted[2],
                    splitted[4].ToLower(),
                    splitted[3]
                ));
            }
        }

        foreach (var department in list)
        {
            department.ChildDepartments = list.Where(x => x.ParentDepartment == department.Name).ToList();
            department.upperDepartments = list.Where(x => x.Name == department.ParentDepartment).ToList();
        }

        return list;
    }
}