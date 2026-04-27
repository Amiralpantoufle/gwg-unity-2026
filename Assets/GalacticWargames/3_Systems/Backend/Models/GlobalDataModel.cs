using System;
using System.Collections.Generic;

[Serializable]
public class GlobalDataResponse
{
    public bool error;
    public GlobalDataOutput output;
}

[Serializable]
public class GlobalDataOutput
{
    public List<Civilisation> civilisations;
    public List<Batiment> batiments;
    public List<Vaisseau> vaisseaux;
}

[Serializable]
public class Civilisation
{
    public int id_civ;
    public string nom_civ;
}

[Serializable]
public class Batiment
{
    public int id_bat;
    public string nom_bat;
}

[Serializable]
public class Vaisseau
{
    public int id_vas;
    public string nom_vas;
}