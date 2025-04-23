import tensorflow as tf

def config_gpu_agents(max_limit=12288):
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
        try:
            tf.config.experimental.set_virtual_device_configuration(
                gpus[0],
                [tf.config.experimental.VirtualDeviceConfiguration(memory_limit=max_limit)]
            )
        except RuntimeError as e:
            print(f"GPU конфигурация уже установлена: {e}")
