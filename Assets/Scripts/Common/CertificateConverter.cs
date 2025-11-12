public static class CertificateConverter
{
    public static Certificates ConvertToEnum(CertificatesAsObjects obj)
    {
        if (obj.sql) return Certificates.Sql;
        if (obj.surf) return Certificates.Surf;
        if (obj.videogames) return Certificates.Videogames;
        return 0;       //add Certificates.None to the enum to have a "default/fallback" option? Then change this line in "return Certificates.None;"
    }

    public static CertificatesAsObjects ConvertToObject(Certificates certificate)
    {
        var obj = new CertificatesAsObjects();

        switch (certificate)
        {
            case Certificates.Sql:
                obj.sql = true;
                break;
            case Certificates.Surf:
                obj.surf = true;
                break;
            case Certificates.Videogames:
                obj.videogames = true;
                break;
            default:
                break;
        }

        return obj;
    }
}
