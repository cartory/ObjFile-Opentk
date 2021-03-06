﻿using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.common
{
    interface IGetters
    {
        Vector3[] GetVerts();
        Vector3[] GetColorData();
        Vector2[] GetTextureCoords();
        int[] GetIndices(int offset = 0);
    }

    interface ISetters
    {
        void setStep(float step);
        void setScale(float scale);
        void setTheta(float theta);
    }

    interface ITransformations
    {
        void Scale(bool plus);

        void RotateX(bool dir);
        void RotateY(bool dir);
        void RotateZ(bool dir);

        void TraslateX(bool dir);
        void TraslateY(bool dir);
        void TraslateZ(bool dir);
    }

    interface IParamTransformations
    {
        void RotateX(float angle);
        void RotateY(float angle);
        void RotateZ(float angle);

        void TraslateX(float distance);
        void TraslateY(float distance);
        void TraslateZ(float distance);
        void Traslate(Vector3 vector);
    }
}
