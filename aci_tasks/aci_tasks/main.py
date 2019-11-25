#!/usr/bin/env python
from config import Config, TaskType
from logger import Logger
from cloud import ContainerGroupInstance, ResourceGroup, ContainerInstance

if __name__ == "__main__":
    config = Config(TaskType.STRONG)
    logger = Logger()

    rg = ResourceGroup(logger, config)
    rg.create()

    cg = ContainerGroupInstance(logger, config)
    cg.create()

    c = ContainerInstance(logger, config)
    c.execute_command('/strong_task/dog.py')
