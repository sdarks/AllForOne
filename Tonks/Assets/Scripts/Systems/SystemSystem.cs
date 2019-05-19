using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSystem : MonoBehaviour
{
	[SerializeField]
	public GameObject PlayerPrefab;

	[SerializeField]
	public GameObject EnemyLeaderPrefab;

	[SerializeField]
	public Component ProjectilePrefab;

	[SerializeField]
	public Transform ProjectilePool;

	[SerializeField]
	public Transform ProjectilePoolActive;

	[SerializeField]
	public Material FriendlyProjectileMaterial;
	[SerializeField]
	public Material EnemyProjectileMaterial;
	[SerializeField]
	public Material FriendlyTrailMaterial;
	[SerializeField]
	public Material EnemyTrailMaterial;

	[SerializeField]
	public AudioClip ProjectileSound;


	public static SystemSystem inst;
    public static int PlayerTeamID = 0;
    public EntityManagementSystem EMSystem;
    public MoveForwardSystem MFSystem;
    public RotateTowardsTargetSystem RTTSystem;
    public FollowPlayerSystem FPSystem;
    public FollowTargetSystem FTSystem;
    public AggroFollowPlayerSystem AFPSystem;
    public DealDamageSystem DDSystem;
    public BreakSystem BSystem;
    public DeathCleanupSystem DCSystem;
    public CreateEntitySystem CESystem;
    public RotateTowardsMouseSystem RTMSystem;
    public FireOnMouseClickSystem FMCSystem;
    public FireSystem FSystem;
    public InputSystem ISystem;
    public PlayerControllerSystem PCSystem;
    public FireConstantlySystem FCSystem;
    public DamageCollisionSystem DACSystem;
    public DamageDealerCleanupSystem DDCSystem;
	public TeamFromFollowTargetSystem TFFTSystem;
	public TeamFromParentSystem TFPSystem;
	public ReplacePlayerSystem RPSystem;
	public ReplaceEnemyLeaderSystem RELSystem;
	public FollowEnemyLeaderSystem FELSystem;
	public RotateTowardsPlayerSystem RTPSystem;
	public ProjectileManagerSystem PMSystem;
	public EntitySpawnerSystem ESSystem;
	public MergeLeaderSystem MLSystem;
    void Awake()
    {
        if (inst)
        {
            Destroy(this);
        }
        else
        {
            inst = this;
            //DontDestroyOnLoad(this);


			EMSystem = new EntityManagementSystem();
            MFSystem = new MoveForwardSystem();
            RTTSystem = new RotateTowardsTargetSystem();
            AFPSystem = new AggroFollowPlayerSystem();
            FPSystem = new FollowPlayerSystem();
            FTSystem = new FollowTargetSystem();
            DDSystem = new DealDamageSystem();
            BSystem = new BreakSystem();
            DCSystem = new DeathCleanupSystem();
            CESystem = new CreateEntitySystem();
            RTMSystem = new RotateTowardsMouseSystem();
            FMCSystem = new FireOnMouseClickSystem();
            FSystem = new FireSystem();
            ISystem = new InputSystem();
            PCSystem = new PlayerControllerSystem();
            FCSystem = new FireConstantlySystem();
            DACSystem = new DamageCollisionSystem();
            DDCSystem = new DamageDealerCleanupSystem();
			TFFTSystem = new TeamFromFollowTargetSystem();
			TFPSystem = new TeamFromParentSystem();
			RPSystem = new ReplacePlayerSystem();
			RELSystem = new ReplaceEnemyLeaderSystem();
			FELSystem = new FollowEnemyLeaderSystem();
			RTPSystem = new RotateTowardsPlayerSystem();
			PMSystem = new ProjectileManagerSystem();
			ESSystem = new EntitySpawnerSystem();
			MLSystem = new MergeLeaderSystem();

			for(int i=0; i<3000; i++)
			{
				Instantiate(ProjectilePrefab, ProjectilePool);
			}
		}

    }

   
    // Update is called once per frame
    void Update()
    {
        /*
         * Current systems flow:
         * - Manage entities
         * - Can target things?
         * - Target things
         * - Respond to targets (move etc)
         * - Deal damage
         * - Damage response
         * - Clear up dead entities etc
         */


        //Manage entities always first!
        EMSystem.SystemUpdate();

		//Spawn enemies
		ESSystem.SystemUpdate();

		//Perf management systems
		PMSystem.SystemUpdate();

		//Input systems
		ISystem.SystemUpdate();
        PCSystem.SystemUpdate();

        //Targeting effecting systems
        AFPSystem.SystemUpdate();

        //Targeting systems
        FPSystem.SystemUpdate();
		FELSystem.SystemUpdate();
        FTSystem.SystemUpdate();
        RTMSystem.SystemUpdate();
        FMCSystem.SystemUpdate();
		TFFTSystem.SystemUpdate();
		TFPSystem.SystemUpdate();
		RTPSystem.SystemUpdate();

        //Movement systems
        MFSystem.SystemUpdate();
        RTTSystem.SystemUpdate();

        //Firing
        FSystem.SystemUpdate();
        FCSystem.SystemUpdate();

        //Damage systems
        DACSystem.SystemUpdate();
        DDSystem.SystemUpdate();

        //Damage response systems
        BSystem.SystemUpdate();
		MLSystem.SystemUpdate();
		RELSystem.SystemUpdate();

		//Final/Cleanup systems
		CESystem.SystemUpdate();
        DCSystem.SystemUpdate();
        DDCSystem.SystemUpdate();
		RPSystem.SystemUpdate();

    }

	public Transform GetProjectileFromPool()
	{
		if(ProjectilePool.childCount<=0)
		{
			for (int i = 0; i < 100; i++)
			{
				Instantiate(ProjectilePrefab, ProjectilePool);
			}
			
		}
		Transform outP = ProjectilePool.GetChild(0);
		outP.SetParent(ProjectilePoolActive);

		ProjectileComponent PC = outP.GetComponent<ProjectileComponent>();
		PC.HasSpawned = false;
		PC.SpawnTime = 0;
		PC.Disabled = false;
		Rigidbody RBody = PC.GetComponent<Rigidbody>();
		if (RBody)
		{
			RBody.detectCollisions = true;
			RBody.isKinematic = false;
			RBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
		return outP;
		
	}
	public void ReturnToPool(Transform projectile)
	{
		projectile.SetParent(ProjectilePool);
	}

    /******************* EVENTS ******************/
    public struct DamageEvent
    {
        public int EntityID;
        public int Damage;
        public Vector3 DamageLocation;

        public DamageEvent(int ID, int Dam, Vector3 DamLoc)
        {
            EntityID = ID;
            Damage = Dam;
            DamageLocation = DamLoc;
        }
    }

    public void DealDamage(int EntityID, int Damage, Vector3 DamageLocation)
    {
        DamageEvent DE = new DamageEvent(EntityID, Damage, DamageLocation);
        DDSystem.DealDamage(DE);
    }

    public struct CreateEntityEvent
    {
        public GameObject Prefab;
        public Transform Parent;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 InitialForce;
        public Vector3 InitialVelocity;
        public int TeamID;
    }

    public void CreateEntity(GameObject inpPrefab, Transform inpParent, Vector3 inpPosition, Quaternion inpRotation, Vector3 inpInitialForce, Vector3 inpInitialVelocity, int TeamID)
    {
        CreateEntityEvent CEE = new CreateEntityEvent();
        CEE.Prefab = inpPrefab;
        CEE.Parent = inpParent;
        CEE.Position = inpPosition;
        CEE.Rotation = inpRotation;
        CEE.InitialForce = inpInitialForce;
        CEE.InitialVelocity = inpInitialVelocity;
        CEE.TeamID = TeamID;

        CESystem.CreateEntity(CEE);
        
    }

	public void CreateProjectile( Transform inpParent, Vector3 inpPosition, Quaternion inpRotation, Vector3 inpInitialForce, Vector3 inpInitialVelocity, int TeamID )
	{
		GameObject obj = GetProjectileFromPool().gameObject;
		if (obj != null)
		{
			obj.SetActive(true);
			obj.transform.position = inpPosition;
			obj.transform.rotation = inpRotation;
			Rigidbody rbody = obj.GetComponent<Rigidbody>();
			if (rbody)
			{
				rbody.velocity = inpInitialVelocity;
				rbody.AddForce(inpInitialForce);
			}
			TeamComponent TC = obj.GetComponent<TeamComponent>();
			if (TC)
			{
				TC.TeamID = TeamID;
			}

			Material normalMaterial;
			Material trailMaterial;
			if(TC.TeamID == SystemSystem.PlayerTeamID)
			{
				normalMaterial = FriendlyProjectileMaterial;
				trailMaterial = FriendlyTrailMaterial;
			}
			else
			{
				normalMaterial = EnemyProjectileMaterial;
				trailMaterial = EnemyTrailMaterial;
			}

			TrailRenderer trailRend = obj.GetComponent<TrailRenderer>();
			MeshRenderer meshRend = obj.GetComponent<MeshRenderer>();
			trailRend.material = trailMaterial;
			meshRend.material = normalMaterial;
		}
	}
}
