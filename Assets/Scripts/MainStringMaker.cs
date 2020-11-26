using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class MainStringMaker : MonoBehaviour {

    public ObiCollisionMaterial collisionMaterial;
    public Material stringMaterial;
    void Start () {
        GameObject solverObject = new GameObject("String Solver", typeof(ObiSolver), typeof(ObiFixedUpdater));
        ObiSolver solver = solverObject.GetComponent<ObiSolver>();
        ObiFixedUpdater updater = solverObject.GetComponent<ObiFixedUpdater>();
        updater.solvers.Add(solver);
        solverObject.AddComponent(typeof(HexPathFinder));
        solver.distanceConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 100);
        solver.particleCollisionConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 30);
        solver.collisionConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 30);
        solver.tetherConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 50);

        // Disable
        solver.skinConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.densityConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.stretchShearConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.stitchConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.volumeConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.bendTwistConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        solver.shapeMatchingConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        // solver.frictionConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);
        // solver.particleFrictionConstraintParameters = new Oni.ConstraintParameters(false, Oni.ConstraintParameters.EvaluationOrder.Sequential, 1);

        // create the blueprint: (ltObiRopeBlueprint, ObiRodBlueprint)
        var blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();
        blueprint.pooledParticles = 20000;
        blueprint.resolution = 1f;

        // Procedurally generate the rope path (a simple straight line):
        blueprint.path.Clear();
        blueprint.path.AddControlPoint(
            new Vector3(0, 0.011f, 0),
            -Vector3.right,
            Vector3.right,
            Vector3.up,
            0.005f,
            0.005f,
            0.03f,
            1,
            Color.white,
            "start"
        );
        blueprint.path.AddControlPoint(
            new Vector3(0, 0.5f, 0),
            -Vector3.right,
            Vector3.right,
            Vector3.up,
            0.005f,
            0.005f,
            0.03f,
            1,
            Color.white,
            "end"
        );
        blueprint.path.FlushEvents();

        blueprint.GenerateImmediate();

        GameObject ropeObject = new GameObject("String", typeof(ObiRope), typeof(ObiRopeExtrudedRenderer));

        ObiRope rope = ropeObject.GetComponent<ObiRope>();
        ObiRopeExtrudedRenderer ropeRenderer = ropeObject.GetComponent<ObiRopeExtrudedRenderer>();
        MeshRenderer ropeMesh = ropeObject.GetComponent<MeshRenderer>();

        if (stringMaterial == null) {
            ropeMesh.material = new Material(Shader.Find("StringShader"));
        } else {
            ropeMesh.material = stringMaterial;
        }
        

        ropeRenderer.section = Resources.Load<ObiRopeSection>("DefaultRopeSection");

        rope.ropeBlueprint = ScriptableObject.Instantiate(blueprint);

        rope.transform.parent = solver.transform;
        // rope.collisionMaterial = collisionMaterial;
        rope.selfCollisions = false;
        rope.stretchingScale = 1;

        ObiRopeCursor cursor = ropeObject.AddComponent(typeof(ObiRopeCursor)) as ObiRopeCursor;
        cursor.cursorMu = 0.5f;
        cursor.sourceMu = 0.5f;

        ObiParticleAttachment groundAttach = ropeObject.AddComponent(typeof(ObiParticleAttachment)) as ObiParticleAttachment;
        ObiParticleAttachment charAttach = ropeObject.AddComponent(typeof(ObiParticleAttachment)) as ObiParticleAttachment;

        groundAttach.target = GameObject.Find("Floor").transform;
        // groundAttach.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        groundAttach.particleGroup = blueprint.groups[0];
        charAttach.target = GameObject.Find("Ariadne").transform;
        charAttach.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        charAttach.particleGroup = blueprint.groups[1];

        ropeObject.AddComponent(typeof(StringController));
    }

    void Update () {
    }
}