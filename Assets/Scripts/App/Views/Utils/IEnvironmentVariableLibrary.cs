﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IEnvironmentVariableLibrary
{
    IEnumerable<EnvironmentVariable> EnvironmentVariables { get; }
}
