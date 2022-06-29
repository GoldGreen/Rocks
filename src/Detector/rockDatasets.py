from detectron2.data.datasets import register_coco_instances
from utils import *

train_dataset_name = "rock_train"
train_images_path = "dataset/train"
train_annotation_path = "dataset/train_annotation.json"

test_dataset_name = "rock_test"
test_images_path = "dataset/test"
test_annotation_path = "dataset/test_annotation.json"

cfg_save_path = "IS_rocks_cfg.pickle"

register_coco_instances(name=train_dataset_name, metadata={},
                        json_file=train_annotation_path, image_root=train_images_path)
register_coco_instances(name=test_dataset_name, metadata={},
                        json_file=test_annotation_path, image_root=test_images_path)


#plot_samples(train_dataset_name, 15)
#plot_samples(test_dataset_name, 5)
