using Application.Logic;

var test = "Отдел программистов";
Console.WriteLine(DepartmentExtensions.GetAllInfo(test));

test = "Бухгалтерия";
Console.WriteLine(DepartmentExtensions.GetAllInfo(test));

test = "Цех ремонта";
Console.WriteLine(DepartmentExtensions.GetAllInfo(test));