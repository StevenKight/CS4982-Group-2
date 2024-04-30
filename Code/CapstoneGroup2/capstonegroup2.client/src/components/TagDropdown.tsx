import React, { useState } from 'react';

import './styles/TagDropdown.css';

type Tag = {
  tagID: number;
  tagName: string;
};

interface Props {
  tags: Tag[];
  getSources(tags: Tag[] |
    null
  ): void;
}

const TagDropdown: React.FC<Props> = ({ tags, getSources }) => {
  const [selectedTags, setSelectedTags] = useState<number[]>([]); // Changed to number[] to store tagIds

  const handleSearch = () => {
    // Implement search logic based on selected tags
    console.log('Search based on selected tags:', selectedTags);
    const selectedTagObjects = selectedTags.map(tagId => tags.find(tag => tag.tagID === tagId)).filter(Boolean);
    getSources(selectedTagObjects as Tag[]);
    
  };

  const handleClearSelection = () => {
    getSources(null);
    setSelectedTags([]);
  };

  const toggleTagSelection = (tagId: number) => {
    if (selectedTags.includes(tagId)) {
      setSelectedTags(selectedTags.filter(id => id !== tagId));
    } else {
      setSelectedTags([...selectedTags, tagId]);
    }
  };

  return (
    <div className='tag-search'>
      <h2 className='tag-search-heading'>Search by Tags:</h2>
      <div className='tag-search-options'>
        <div className='tag-search-tags'>
          {
            tags.map(tag => (
              <label key={tag.tagID}>
                <input
                  type="checkbox"
                  value={String(tag.tagID)}
                  checked={selectedTags.includes(tag.tagID)}
                  onChange={() => toggleTagSelection(tag.tagID)}
                />
                {tag.tagName}
              </label>
            ))
          }
        </div>
        <div>
          <button disabled={selectedTags.length == 0} onClick={handleSearch}>Search</button>
          <button disabled={selectedTags.length == 0} onClick={handleClearSelection}>Clear Selection</button>
        </div>
      </div>
    </div>
  );
};

export default TagDropdown;