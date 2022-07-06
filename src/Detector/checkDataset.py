from detectron2.data.datasets import register_coco_instances
from detectron2.data import DatasetCatalog, MetadataCatalog
import cv2
from detectron2.utils.visualizer import Visualizer
import matplotlib.pyplot as plt

train_dataset_name = "rock_train"
train_images_path = "dataset/train"
train_annotation_path = "dataset/train_annotation.json"

register_coco_instances(name=train_dataset_name, metadata={},
                        json_file=train_annotation_path, image_root=train_images_path)

dataset = DatasetCatalog.get(train_dataset_name)
dataset_metadata = MetadataCatalog.get(train_dataset_name)

for s in dataset:
    print(s["file_name"])
    img = cv2.imread(s["file_name"])
    v = Visualizer(img[:, :, ::-1], metadata=dataset_metadata, scale=0.5)
    v = v.draw_dataset_dict(s)
    plt.figure(figsize=(15, 20))
    plt.imshow(v.get_image())
    wm = plt.get_current_fig_manager()
    wm.window.state('zoomed')
    plt.title(s["file_name"])
    plt.show()
