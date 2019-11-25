#!/usr/bin/env python
from config import Config, TaskType
from logger import Logger
from cloud import ContainerGroupInstance, ResourceGroup

if __name__ == "__main__":
    config = Config(TaskType.STRONG)
    logger = Logger()

    rg = ResourceGroup(logger, config)
    rg.create()

    cg = ContainerGroupInstance(logger, config)
    cg.create()
