#!/usr/bin/env python
import os
import zipfile
import tempfile
from multiprocessing.pool import ThreadPool
from azure.storage.blob import BlobServiceClient
from azure.core.exceptions import ResourceExistsError

PROJECT_FILE_PATH = 'src/data/files.zip'
container_name = PROJECT_FILE_PATH.rsplit('/', maxsplit=1)[-1].replace('.', '-')
conn_string = os.environ["AZURE_STORAGE_CONNECTION_STRING"]
blob_svc_client = BlobServiceClient.from_connection_string(conn_string)

def upload_file(file_name):
    """ Create blob with same name as local file name"""
    blob_client = blob_svc_client.get_blob_client(container=container_name, 
                                                    blob=file_name.rsplit('/', maxsplit=1)[-1])

    # Create blob on storage
    print(f'uploading file - {file_name}')
    with open(file_name, "rb") as data:
        blob_client.upload_blob(data, overwrite=True)
    return file_name


def upload_all(all_file_names):
    """Upload 10 files at a time!"""
    with ThreadPool(processes=int(10)) as pool:
        return pool.map(upload_file, all_file_names)


if __name__ == "__main__":
    # Create tmp Dir
    with tempfile.TemporaryDirectory() as tmpdir:
        # unzip files to tmpdir
        with zipfile.ZipFile(PROJECT_FILE_PATH) as zip_ref:
            zip_ref.extractall(tmpdir)

        # upload files to blob
        container_client = blob_svc_client.get_container_client(container_name)

        try:
            container_client.create_container()
        except ResourceExistsError as e:
            pass

        all_files = [os.path.join(tmpdir, f) for f in os.listdir(tmpdir)]

        result = upload_all(all_files)
        print(result)
