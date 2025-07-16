namespace Challenge.Skopia.TaskManagement.Infrastructure.Extensions;

public static class StringExtension
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string OnlyNumbers(this string value)
    {
        //return new String(value.Where(Char.IsDigit).ToArray());
        return new string(value?.Where(item => char.IsDigit(item)).ToArray());
    }

    /// <summary>
    /// Um tradutor de nomes que converte nomes CLR (class, propriedade ou campo, struct, enum) padrão em nomes de banco de dados no formato Snake-Case (p.ex., de MinhaClasse para minha_classe)
    /// <br></br>
    /// A ideia foi retirada <see href="https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/">deste artigo</see>,
    /// <see href="https://github.com/npgsql/efcore.pg/issues/21#issuecomment-376114368"> deste issue. </see>
    /// </summary>
    /// <param name="clrName"></param>
    /// <returns></returns>
    public static string ToSnakeCase(this string clrName)
    {
        if (string.IsNullOrEmpty(clrName))
            return clrName;

        return string.Concat(clrName.Select((item, index)
            => index > 0 && char.IsUpper(item)
                ? $"_{item}"
                : item.ToString()
            )).ToLower();
    }

    public static string UppercaseFirst(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        char[] arr = str.ToCharArray();
        arr[0] = char.ToUpper(arr[0]);

        return new string(arr);
    }
}