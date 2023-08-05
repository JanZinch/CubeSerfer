using Controllers;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private Rigidbody ConnectedBody
        {
            get =>_joint.connectedBody;
            set => _joint.connectedBody = value;
        }
        
        public void AttachTo(Block other)
        {
            
            //transform.position = other.GetAttachingPosition();
            //_rigidbody.position = other.GetAttachingPosition();

            
            Destroy(_joint);
       
            transform.position = other.GetAttachingPosition();
            //_rigidbody.MovePosition(other.GetAttachingPosition());

            _joint = gameObject.AddComponent<FixedJoint>();
            other.ConnectedBody = _rigidbody;
            
            //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = other.GetAttachingPosition();
            
            //transform.rotation = Quaternion.identity;


            /*DOVirtual.DelayedCall(1.0f, () =>
            {
                other.ConnectedBody = _rigidbody;
            });*/
        }
        
        
        

        public void PutCharacter(Character character)
        {
            character.transform.position = GetAttachingPosition();
            character.transform.rotation = Quaternion.identity;
            
            _joint.connectedBody = character.Rigidbody;
        }

        private Vector3 GetAttachingPosition()
        {
            //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y * 1.15f, 0.0f);
            
            return transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y, 0.0f);
        }

    }
}