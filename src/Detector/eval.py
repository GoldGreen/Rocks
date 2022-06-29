from rockDatasets import *
from detectron2.evaluation import COCOEvaluator, inference_on_dataset
from detectron2.data import build_detection_test_loader
from detectron2.engine import DefaultPredictor
import os
import pickle

cfg_save_path = "IS_rocks_cfg.pickle"
with open(cfg_save_path, 'rb') as f:
    cfg = pickle.load(f)

cfg.MODEL.WEIGHTS = os.path.join(cfg.OUTPUT_DIR, "model_0019999.pth")
cfg.MODEL.ROI_HEADS.SCORE_THRESH_TEST = 0.5


def main():
    predictor = DefaultPredictor(cfg)

    dataset_name = train_dataset_name
    evaluator = COCOEvaluator(dataset_name, cfg,
                              False, output_dir=cfg.OUTPUT_DIR
                              )

    val_loader = build_detection_test_loader(cfg, dataset_name)
    inference_on_dataset(predictor.model, val_loader, evaluator)


if __name__ == "__main__":
    main()
