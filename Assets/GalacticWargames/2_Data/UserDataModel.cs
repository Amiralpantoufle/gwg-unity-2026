using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


[Serializable]
public class UserDataOutput
{
    public List<OwnerEntiteSpatiale> owner_entite_spatiales;
    public List<OesRessource> oes_ressources_oer;
    public List<OesVaisseau> oes_vaisseau_ova;

    public List<ConstructionVaisseau> construction_vaisseau;
    public List<ConstructionBatiment> construction_batiment;

    public List<DiscoveredEntiteSpatiale> discoveredEntiteSpatiales;
    public List<ProductionEsp> productionEsp;

    public List<Flotte> flottes;
    public List<FlotteVaisseau> flotte_vaisseaux;
    public List<FlotteRessource> flotte_ressources;

    public List<Reward> reward;
    public List<RewardDetail> reward_details;

    public List<Mouvement> mouvements;
    public List<Combat> combats;

    public List<TitreHonorifique> titre_honorifiques;
    public List<Punchline> punchlines;
    public List<Distinction> distinctions;

    public List<Flotte> autre_flottes;
    public List<FlotteVaisseau> autre_flotte_vaisseaux;
    public List<Mouvement> autre_mouvements;

    public UserInfos infos_user;
    public OnboardingStatus onboarding_status;
}

[Serializable]
public class OwnerEntiteSpatiale
{
    public int id_oes;
    public int idesp_oes;
    public int idusr_oes;
    public int? idbat_oes;
    public int idsba_oes;
    public string nom_oes;
    public int statut_oes;
    public int nb_files_construction_oes;
    public string date_loose_oes;
    public string created_at;
    public string updated_at;
}

[Serializable]
public class OesRessource
{
    public int id_oer;
    public int idoes_oer;
    public int idmtx_oer;
    public int nombre_oer;
}

[Serializable]
public class ProductionEsp
{
    public int id_pep;
    public int idesp_pep;
    public int idmtx_pep;
    public int production_pep;
}

[Serializable]
public class Flotte
{
    public int id_flt;
    public int idusr_flt;
    public int idesp_flt;

    public int statut_flt;
    public int mode_flt;

    public bool combat_en_cours_flt;
    public bool en_mvt_flt;

    public int? idflt_fusion_flt;

    public string name_flt;

    public int stockage_total_flt;
    public int stockage_use_flt;

    public bool a_gagne_ressources_flt;
    public bool is_garrison_flt;
}

[Serializable]
public class FlotteVaisseau
{
    public int id_fvs;
    public int idflt_fvs;
    public int idvas_fvs;

    public int? idskn_fvs;

    public int coordonnees_x_fvs;
    public int coordonnees_y_fvs;

    public int nombre_fvs;
}

[Serializable]
public class FlotteRessource
{
    public int id_fre;
    public int idflt_fre;
    public int idmtx_fre;
    public int nombre_fre;
}

[Serializable]
public class Reward
{
    public int id_rwd;
    public int idusr_rwd;
    public int source_rwd;
    public bool claim_rwd;
}

[Serializable]
public class RewardDetail
{
    public int id_rdd;
    public int idrwd_rdd;
    public int type_rdd;
    public int? idexterne_rdd;
    public int nombre_rdd;
}

[Serializable]
public class UserInfos
{
    public string name;
    public int id;
    public int? id_all;
    public int idciv_usr;

    public Level_Infos level_progress;

    public int BASE_STOCKAGE_DEFAULT;
    public int TYPE_NOMBRE_FLOTTE_MAX;
    public int TYPE_NOMBRE_OES_MAX;

    public OnboardingStatus onboarding;
}
[Serializable]
public class Level_Infos
{
    public int current_level;
    public int xp_total;
    public int xp_in_level;
    public int xp_for_next_level;
}

[Serializable]
public class OnboardingStatus
{
    public int user_id;
    public string status;
    public string status_label;

    public int completion_rate;
    public string next_action;

    public bool is_account_active;
    public bool email_verified;
    public bool civilization_selected;

    public List<TutorialInfo> pending_mandatory_tutorials;
    public List<OnboardingStep> steps;
}

[Serializable]
public class TutorialInfo
{
    public int id;
    public string name;
    public string category;
}

[Serializable]
public class OnboardingStep
{
    public string code;
    public string label;
    public bool done;
}

[Serializable] public class OesVaisseau { }
[Serializable] public class ConstructionVaisseau { }
[Serializable] public class ConstructionBatiment { }
[Serializable] public class DiscoveredEntiteSpatiale { }
[Serializable] public class Mouvement { }
[Serializable] public class Combat { }
[Serializable] public class TitreHonorifique { }
[Serializable] public class Punchline { }
[Serializable] public class Distinction { }